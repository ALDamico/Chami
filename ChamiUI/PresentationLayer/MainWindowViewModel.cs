using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ChamiUI.BusinessLayer;
using ChamiUI.BusinessLayer.Factories;
using ChamiUI.DataLayer.Entities;
using ChamiUI.PresentationLayer.Progress;

namespace ChamiUI.PresentationLayer
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

        public MainWindowViewModel(string connectionString)
        {
            _dataAdapter = new EnvironmentDataAdapter(connectionString);
            Environments = GetEnvironments();
            EditingEnabled = false;
            if (Environments.Any())
            {
                SelectedEnvironment = Environments.First();
            }
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
    }
}