using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChamiUI.BusinessLayer;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.BusinessLayer.Factories;
using ChamiUI.DataLayer.Entities;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.Progress;
using dotenv.net;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private bool _editingEnabled;

        public bool EditingEnabled
        {
            get => _editingEnabled;
            private set
            {
                _editingEnabled = value;
                OnPropertyChanged(nameof(EditingEnabled));
            }
        }

        public void EnableEditing()
        {
            EditingEnabled = true;
        }
        
        public void DisableEditing()
        {
            EditingEnabled = false;
        }

        private SettingsViewModel _settings;

        public SettingsViewModel Settings
        {
            get => _settings;
            set
            {
                _settings = value;
                OnPropertyChanged(nameof(Settings));
            }
        }

        public MainWindowViewModel(string connectionString)
        {
            _dataAdapter = new EnvironmentDataAdapter(connectionString);
            _settingsDataAdapter = new SettingsDataAdapter(connectionString);
            Environments = GetEnvironments();
            EditingEnabled = false;
            if (Environments.Any())
            {
                SelectedEnvironment = Environments.First();
            }

            Settings = GetSettingsViewModel();
        }

        private SettingsDataAdapter _settingsDataAdapter;

        private SettingsViewModel GetSettingsViewModel()
        {
            return _settingsDataAdapter.GetSettings();
        }

        public async Task ChangeEnvironmentAsync(IProgress<CmdExecutorProgress> progress = null)
        {
            var cmdExecutor = new CmdExecutor();
            var currentEnvironmentName = System.Environment.GetEnvironmentVariable("_CHAMI_ENV");
            if (currentEnvironmentName != null)
            {
                var currentOsEnvironment = _dataAdapter.GetEnvironmentEntityByName(currentEnvironmentName);
                foreach (var environmentVariable in currentOsEnvironment.EnvironmentVariables)
                {
                    var newCommand =
                        EnvironmentVariableCommandFactory.GetCommand(typeof(EnvironmentVariableRemovalCommand),
                            environmentVariable);
                    cmdExecutor.AddCommand(newCommand);
                }
            }

            var newEnvironment = _dataAdapter.GetEnvironmentEntityByName(SelectedEnvironment.Name);
            cmdExecutor.AddCommand(EnvironmentVariableCommandFactory.GetCommand(
                typeof(EnvironmentVariableApplicationCommand),
                new EnvironmentVariable() {Name = "_CHAMI_ENV", Value = SelectedEnvironment.Name}));

            foreach (var environmentVariable in newEnvironment.EnvironmentVariables)
            {
                var newCommand = EnvironmentVariableCommandFactory.GetCommand(
                    typeof(EnvironmentVariableApplicationCommand),
                    environmentVariable);
                cmdExecutor.AddCommand(newCommand);
            }

            await cmdExecutor.ExecuteAsync(progress);
            ;
        }

        public void ChangeEnvironment(IProgress<CmdExecutorProgress> progress = null)
        {
            ChangeEnvironmentAsync().GetAwaiter().GetResult();
        }

        public ObservableCollection<EnvironmentViewModel> Environments { get; set; }

        private EnvironmentViewModel _selectedEnvironment;

        public EnvironmentViewModel SelectedEnvironment
        {
            get => _selectedEnvironment;

            set
            {
                _selectedEnvironment = value;
                OnPropertyChanged(nameof(SelectedEnvironment));
            }
        }

        public ObservableCollection<EnvironmentViewModel> GetEnvironments()
        {
            Environments = new ObservableCollection<EnvironmentViewModel>(_dataAdapter.GetEnvironments());
            return Environments;
        }

        private readonly EnvironmentDataAdapter _dataAdapter;

        public void DeleteSelectedEnvironment()
        {
            _dataAdapter.DeleteEnvironment(SelectedEnvironment);
            Environments.Remove(SelectedEnvironment);
            SelectedEnvironment = null;
        }

        public void SaveCurrentEnvironment()
        {
            _dataAdapter.SaveEnvironment(SelectedEnvironment);
            DisableEditing();
        }

        public event EventHandler<EnvironmentExistingEventArgs> EnvironmentExists;

        public void ImportJson(Stream file)
        {
            var environmentJsonProcessor = new EnvironmentJsonReader(file);
            var environment = environmentJsonProcessor.Process();
            if (environment == null) return;
            if (!CheckEnvironmentExists(environment))
            {
                Environments.Add(environment);
                SelectedEnvironment = environment;
                EnableEditing();
            }
        }

        protected bool CheckEnvironmentExists(EnvironmentViewModel environment)
        {
            if (Environments.Any(e => e.Name == environment.Name))
            {
                EnvironmentExists?.Invoke(this, new EnvironmentExistingEventArgs(environment.Name));
                return true;
            }

            return false;
        }

        public void BackupEnvironment()
        {
            _dataAdapter.BackupEnvironment();
        }

        public void ImportDotEnv(string filePath)
        {
            var newVariables = DotEnv.Fluent().WithEnvFiles(new[] {filePath}).Read();
            var environmentViewModel = new EnvironmentViewModel();
            environmentViewModel.Name = filePath;
            foreach (var variable in newVariables)
            {
                var environmentVariable = new EnvironmentVariableViewModel();
                environmentVariable.Name = variable.Key;
                environmentVariable.Value = variable.Value;
                environmentViewModel.EnvironmentVariables.Add(environmentVariable);
            }

            Environments.Add(environmentViewModel);
            SelectedEnvironment = environmentViewModel;
            EnableEditing();
            _dataAdapter.InsertEnvironment(environmentViewModel);
        }
    }
}