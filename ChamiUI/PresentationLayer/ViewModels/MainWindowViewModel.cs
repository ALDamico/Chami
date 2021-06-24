using ChamiUI.BusinessLayer;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.BusinessLayer.Factories;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.Progress;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using Chami.Db.Entities;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.Filtering;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public void CancelActiveTask()
        {
            CancellationTokenSource.Cancel();
        }
        private bool _editingEnabled;
        private CollectionViewSource EnvironmentsViewSource { get; }

        public bool EditingEnabled
        {
            get => _editingEnabled;
            private set
            {
                _editingEnabled = value;
                OnPropertyChanged(nameof(EditingEnabled));
                OnPropertyChanged(nameof(ExecuteButtonPlayEnabled));
                OnPropertyChanged(nameof(ExecuteButtonIcon));
            }
        }
        public ObservableCollection<IFilterStrategy> FilterStrategies { get; }

        public void EnableEditing()
        {
            EditingEnabled = true;
        }

        public void DisableEditing()
        {
            EditingEnabled = false;
            // We're using the SelectedVariable property to tell the application that every edit has been completed and
            // it's okay to try to save
            SelectedVariable = null;
        }

        public void ChangeFilterStrategy(IFilterStrategy filterStrategy)
        {
            var currentFilterStrategy = FilterStrategy;
            filterStrategy.SearchedText = currentFilterStrategy.SearchedText;
            filterStrategy.Comparison = currentFilterStrategy.Comparison;
            FilterStrategy = filterStrategy;
        }

        private IFilterStrategy _filterStrategy;

        public IFilterStrategy FilterStrategy
        {
            get => _filterStrategy;
            set
            {
                _filterStrategy = value;
                OnPropertyChanged(nameof(FilterStrategy));
            }
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

        public bool IsClearFilterButtonEnabled => FilterStrategy.SearchedText != null;

        private string _filterText;

        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                FilterStrategy.SearchedText = value;
                OnPropertyChanged(nameof(FilterText));
                OnPropertyChanged(nameof(IsClearFilterButtonEnabled));
                OnPropertyChanged(nameof(ClearFilterTextButtonIcon));
            }
        }

        public MainWindowViewModel(string connectionString)
        {
            _dataAdapter = new EnvironmentDataAdapter(connectionString);
            _settingsDataAdapter = new SettingsDataAdapter(connectionString);
            _watchedApplicationDataAdapter = new WatchedApplicationDataAdapter(connectionString);
            Environments = GetEnvironments();
            EditingEnabled = false;
            if (Environments.Any())
            {
                SelectedEnvironment = ActiveEnvironment ?? Environments.First();
            }

            Settings = GetSettingsViewModel();
            EnvironmentsViewSource = new CollectionViewSource {Source = Environments};

            FilterStrategies = new ObservableCollection<IFilterStrategy>();
            InitFilterStrategies();
            CancellationTokenSource = new CancellationTokenSource();
        }

        private void InitFilterStrategies()
        {
            FilterStrategy = new EnvironmentNameFilterStrategy();
            FilterStrategies.Add(FilterStrategy);
            FilterStrategies.Add(new EnvironmentAndVariableNameFilterStrategy());
            FilterStrategies.Add(new EnvironmentAndVariableNameAndValueFilterStrategy());
        }

        private bool _isDescendingSorting;
        public bool IsDescendingSorting
        {
            get => _isDescendingSorting;
            set
            {
                _isDescendingSorting = value;
                OnPropertyChanged(nameof(IsDescendingSorting));
            }
        }

        public void SetSortDescription(SortDescription sortDescription)
        {
            EnvironmentsViewSource.SortDescriptions.Clear();
            EnvironmentsViewSource.SortDescriptions.Add(sortDescription);
        }

        public event EventHandler<EnvironmentChangedEventArgs> EnvironmentChanged;

        public bool ExecuteButtonPlayEnabled => !EditingEnabled && !_isChangeInProgress;

        private readonly SettingsDataAdapter _settingsDataAdapter;
        private readonly WatchedApplicationDataAdapter _watchedApplicationDataAdapter;

        private SettingsViewModel GetSettingsViewModel()
        {
            var settings = _settingsDataAdapter.GetSettings();
            var watchedApplications = _watchedApplicationDataAdapter.GetActiveWatchedApplications();
            settings.WatchedApplicationSettings.WatchedApplications =
                new ObservableCollection<WatchedApplicationViewModel>(watchedApplications);
            return settings;
        }

        private void OnEnvironmentChanged(object sender, EnvironmentChangedEventArgs args)
        {
            if (args != null)
            {
                ActiveEnvironment = args.NewActiveEnvironment;
                SelectedEnvironment = ActiveEnvironment;
            }
            else
            {
                ActiveEnvironment = null;
            }

            ChangeActiveEnvironment();
            EnvironmentChanged?.Invoke(this, args);
        }

        private void ChangeActiveEnvironment()
        {
            foreach (var environment in Environments)
            {
                environment.IsActive = ActiveEnvironment != null && ActiveEnvironment.Name == environment.Name;
            }
        }

        private bool _isChangeInProgress;

        private void SetIsChangeInProgress(bool value)
        {
            _isChangeInProgress = value;
            OnPropertyChanged(nameof(ExecuteButtonPlayEnabled));
            OnPropertyChanged(nameof(ExecuteButtonIcon));
        }

        public bool AreSelectedEnvironmentVariablesValid()
        {
            return SelectedEnvironment.EnvironmentVariables.All(envVar =>
                envVar.IsValid == null || envVar.IsValid == true);
        }

        private CancellationToken GetNewCancellationToken()
        {
            CancellationTokenSource = new CancellationTokenSource();
            return CancellationTokenSource.Token;
        }

        public async Task ChangeEnvironmentAsync(IProgress<CmdExecutorProgress> progress)
        {
            SetIsChangeInProgress(true);
            var cmdExecutor = new CmdExecutor(SelectedEnvironment);
            cmdExecutor.EnvironmentChanged += OnEnvironmentChanged;
            var currentEnvironmentName = System.Environment.GetEnvironmentVariable("_CHAMI_ENV");
            var cancellationToken = GetNewCancellationToken();
            if (currentEnvironmentName != null)
            {
                var currentOsEnvironment = _dataAdapter.GetEnvironmentEntityByName(currentEnvironmentName);
                // currentOsEnvironment could be null in case there's a stray _CHAMI_ENV environment variable but no 
                // corresponding entity
                if (currentOsEnvironment != null)
                {
                    foreach (var environmentVariable in currentOsEnvironment.EnvironmentVariables)
                    {
                        var newCommand =
                            EnvironmentVariableCommandFactory.GetCommand(typeof(EnvironmentVariableRemovalCommand),
                                environmentVariable);
                        cmdExecutor.AddCommand(newCommand);
                    }
                }
            }

            var newEnvironment = _dataAdapter.GetEnvironmentEntityById(SelectedEnvironment.Id);
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

            await cmdExecutor.ExecuteAsync(progress, cancellationToken);
            SetIsChangeInProgress(false);
        }
        
        public CancellationTokenSource CancellationTokenSource { get; set; }

        private EnvironmentViewModel _activeEnvironment;

        public EnvironmentViewModel ActiveEnvironment
        {
            get => _activeEnvironment;
            set
            {
                _activeEnvironment = value;
                OnPropertyChanged(nameof(ActiveEnvironment));
                OnPropertyChanged(nameof(WindowTitle));
            }
        }

        public void ChangeEnvironment(IProgress<CmdExecutorProgress> progress)
        {
            ChangeEnvironmentAsync(progress).GetAwaiter().GetResult();
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
                OnPropertyChanged(nameof(SelectedVariable));
                OnPropertyChanged(nameof(ExecuteButtonPlayEnabled));
                OnPropertyChanged(nameof(ExecuteButtonIcon));
            }
        }

        public string ExecuteButtonIcon
        {
            get
            {
                if (SelectedEnvironment == null)
                {
                    return "/Assets/Svg/play_disabled.svg";
                }
                
                if (ExecuteButtonPlayEnabled)
                {
                    return "/Assets/Svg/play.svg";
                }

                

                return "/Assets/Svg/stop.svg";
            }
        }
        
        public string ClearFilterTextButtonIcon
        {
            get
            {
                if (IsClearFilterButtonEnabled)
                {
                    return "/Assets/Svg/times.svg";
                }

                return "/Assets/Svg/times_disabled.svg";
            }
        }

        public EnvironmentVariableViewModel SelectedVariable
        {
            get => _selectedVariable;
            set
            {
                _selectedVariable = value;
                OnPropertyChanged(nameof(SelectedVariable));
                OnPropertyChanged(nameof(SelectedEnvironment.EnvironmentVariables));
            }
        }

        private EnvironmentVariableViewModel _selectedVariable;

        private ObservableCollection<EnvironmentViewModel> GetEnvironments()
        {
            Environments = new ObservableCollection<EnvironmentViewModel>(_dataAdapter.GetEnvironments());
            return Environments;
        }

        private readonly EnvironmentDataAdapter _dataAdapter;

        public string GetDetectedApplicationsMessage()
        {
            var watchedApplicationSettings = Settings.WatchedApplicationSettings;
            if (watchedApplicationSettings.IsDetectionEnabled)
            {
                var applicationDetector =
                    new RunningApplicationDetector(watchedApplicationSettings.WatchedApplications);
                var detectedApplications = applicationDetector.Detect();
                if (detectedApplications is {Count: > 0})
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine(
                        ChamiUIStrings.DetectorMessageBoxTextPart1);
                    foreach (var detectedApplication in detectedApplications)
                    {
                        var processName = detectedApplication.ProcessName;
                        if (string.IsNullOrWhiteSpace(processName))
                        {
                            processName = detectedApplication.Name;
                        }

                        stringBuilder.AppendLine(processName);
                    }

                    stringBuilder.Append(ChamiUIStrings.DetectorMessageBoxTextPart2);
                    return stringBuilder.ToString();
                }
            }

            return null;
        }

        public void DeleteSelectedEnvironment()
        {
            _dataAdapter.DeleteEnvironment(SelectedEnvironment);
            Environments.Remove(SelectedEnvironment);
            SelectedEnvironment = null;
        }

        public void SaveCurrentEnvironment()
        {
            var environment = _dataAdapter.SaveEnvironment(SelectedEnvironment);
            SelectedEnvironment = environment;
            OnPropertyChanged(nameof(Environments));
            OnPropertyChanged(nameof(SelectedEnvironment));
            DisableEditing();
        }

        public event EventHandler<EnvironmentExistingEventArgs> EnvironmentExists;

        private string _windowTitle = "Chami";

        public string WindowTitle
        {
            get
            {
                if (ActiveEnvironment != null)
                {
                    return $"{_windowTitle} - {ActiveEnvironment.Name}";
                }
                return _windowTitle;
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

        public void DeleteSelectedVariable()
        {
            _dataAdapter.DeleteVariable(SelectedVariable);
            SelectedEnvironment.EnvironmentVariables.Remove(SelectedVariable);
            DisableEditing();
        }

        public async Task ResetEnvironmentAsync(IProgress<CmdExecutorProgress> progress, CancellationToken cancellationToken)
        {
            if (progress != null)
            {
                CmdExecutorProgress executorProgress =
                    new CmdExecutorProgress(0, null, ChamiUIStrings.RevertToOriginalEnvironmentMessage);
                progress.Report(executorProgress);
            }
            var cmdExecutor = new CmdExecutor();
            var detector = new EnvironmentVariableRegistryRetriever();
            
            var currentEnvironmentName = detector.GetEnvironmentVariable("_CHAMI_ENV");
            if (currentEnvironmentName != null)
            {
                var currentOsEnvironment = _dataAdapter.GetEnvironmentEntityByName(currentEnvironmentName);
                // currentOsEnvironment could be null in case there's a stray _CHAMI_ENV environment variable but no 
                // corresponding entity
                if (currentOsEnvironment != null)
                {
                    foreach (var environmentVariable in currentOsEnvironment.EnvironmentVariables)
                    {
                        var newCommand =
                            EnvironmentVariableCommandFactory.GetCommand(typeof(EnvironmentVariableRemovalCommand),
                                environmentVariable);
                        cmdExecutor.AddCommand(newCommand);
                    }
                    var chamiEnvVariable = new EnvironmentVariable() {Name = "_CHAMI_ENV"};
                    var chamiEnvVarRemovalCommand =
                        EnvironmentVariableCommandFactory.GetCommand(typeof(EnvironmentVariableRemovalCommand),
                            chamiEnvVariable);
                    cmdExecutor.AddCommand(chamiEnvVarRemovalCommand);
                    await cmdExecutor.ExecuteAsync(progress, cancellationToken);
                }
            }
            else
            {
                if (progress != null)
                {
                    CmdExecutorProgress executorProgress = new CmdExecutorProgress(100, null,
                        ChamiUIStrings.RevertToOriginalEnvironmentNop);
                    progress.Report(executorProgress);
                }
            }
            OnEnvironmentChanged(this, new EnvironmentChangedEventArgs(null));
        }

        internal EnvironmentVariableViewModel CreateEnvironmentVariable()
        {
            var newVariable = new EnvironmentVariableViewModel {Environment = SelectedEnvironment};
            SelectedVariable = newVariable;
            //SelectedEnvironment.EnvironmentVariables.Add(newVariable);
            return newVariable;
        }

        public void DetectCurrentEnvironment()
        {
            var detector = new EnvironmentVariableRegistryRetriever();
            
            var currentEnvironmentName = detector.GetEnvironmentVariable("_CHAMI_ENV");
            OnEnvironmentChanged(this,
                new EnvironmentChangedEventArgs(Environments.FirstOrDefault(e => e.Name == currentEnvironmentName)));
        }

        public void ResetCurrentEnvironmentFromDatasource()
        {
            if (SelectedEnvironment != null)
            {
                SelectedEnvironment = _dataAdapter.GetEnvironmentById(SelectedEnvironment.Id);
            }
        }

        public async Task RenameEnvironment(string argsNewName, Progress<CmdExecutorProgress> progress)
        {
            //SelectedEnvironment.Name = argsNewName;
            var environmentToSave = SelectedEnvironment;
            environmentToSave.Name = argsNewName;
            var newSelectedEnvironment = _dataAdapter.SaveEnvironment(environmentToSave);
            Environments = GetEnvironments();
            OnPropertyChanged(nameof(Environments));

            SelectedEnvironment = newSelectedEnvironment;
            if (SelectedEnvironment.Equals(ActiveEnvironment))
            {
                ActiveEnvironment = newSelectedEnvironment;
                await ChangeEnvironmentAsync(progress);
            }
            SelectedEnvironment.Name = argsNewName;
        }

        public List<EnvironmentViewModel> StartImportFiles(string[] dialogFileNames)
        {
            List<EnvironmentViewModel> output = new List<EnvironmentViewModel>();
            foreach (var fileName in dialogFileNames)
            {
                var reader = EnvironmentReaderFactory.GetEnvironmentReaderByExtension(fileName);
                var viewModel = reader.Process();
                output.Add(viewModel);
            }

            return output;
        }
    }
}