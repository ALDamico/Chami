using ChamiUI.BusinessLayer;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.BusinessLayer.Factories;
using ChamiUI.PresentationLayer.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Chami.CmdExecutor;
using Chami.CmdExecutor.Progress;
using Chami.Db.Entities;
using ChamiUI.BusinessLayer.Commands;
using ChamiUI.BusinessLayer.Exceptions;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.Converters;
using ChamiUI.PresentationLayer.Filtering;
using ChamiUI.PresentationLayer.Minimizing;
using ChamiUI.Windows.DetectedApplicationsWindow;
using Newtonsoft.Json;
using ChamiUI.PresentationLayer.Utils;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// How the window should behave when it's minimized.
        /// </summary>
        public IMinimizationStrategy MinimizationStrategy => _settings.MinimizationBehaviour.MinimizationStrategy;

        /// <summary>
        /// Cancels the execution of the active <see cref="CmdExecutor"/> queue.
        /// </summary>
        public void CancelActiveTask()
        {
            _cancellationTokenSource.Cancel();
        }

        private bool _editingEnabled;

        /// <summary>
        /// Determines if the user is in the process of editing an environment.
        /// </summary>
        public bool EditingEnabled
        {
            get => _editingEnabled;
            private set
            {
                _editingEnabled = value;
                OnPropertyChanged(nameof(EditingEnabled));
                OnPropertyChanged(nameof(ExecuteButtonPlayEnabled));
                OnPropertyChanged(nameof(ExecuteButtonIcon));
                OnPropertyChanged(nameof(IsDatagridReadonly));
            }
        }

        public bool IsDatagridReadonly => !EditingEnabled;

        /// <summary>
        /// A list of available <see cref="FilterStrategies"/> for use by the filtering component.
        /// </summary>
        public ObservableCollection<IFilterStrategy> FilterStrategies { get; }

        /// <summary>
        /// Starts the editing process, which changes of the window behaves.
        /// </summary>
        public void EnableEditing()
        {
            EditingEnabled = true;
        }

        /// <summary>
        /// Terminates the editing process.
        /// </summary>
        public void DisableEditing()
        {
            EditingEnabled = false;
            // We're using the SelectedVariable property to tell the application that every edit has been completed and
            // it's okay to try to save
            //SelectedVariable = null;
        }

        /// <summary>
        /// Change the filter strategy of the filtering component.
        /// </summary>
        /// <param name="filterStrategy">The new filter strategy to apply.</param>
        public void ChangeFilterStrategy(IFilterStrategy filterStrategy)
        {
            var currentFilterStrategy = FilterStrategy;
            filterStrategy.SearchedText = currentFilterStrategy.SearchedText;
            filterStrategy.Comparison = currentFilterStrategy.Comparison;
            FilterStrategy = filterStrategy;
        }

        private IFilterStrategy _filterStrategy;

        /// <summary>
        /// The filter strategy to use when filtering the environment listview.
        /// </summary>
        public IFilterStrategy FilterStrategy
        {
            get => _filterStrategy;
            set
            {
                _filterStrategy = value;
                OnPropertyChanged(nameof(FilterStrategy));
                OnPropertyChanged(nameof(FilterStrategyComboboxToolTip));
                OnPropertyChanged(nameof(IsCaseSensitiveSearch));
            }
        }

        private SettingsViewModel _settings;

        /// <summary>
        /// Contains all the settings available to the application.
        /// </summary>
        public SettingsViewModel Settings
        {
            get => _settings;
            set
            {
                _settings = value;
                OnPropertyChanged(nameof(Settings));
                OnPropertyChanged(nameof(MinimizationStrategy));
            }
        }

        /// <summary>
        /// Determines if the clear filter button (the big red cross) is enabled or not.
        /// </summary>
        public bool IsClearFilterButtonEnabled => FilterStrategy.SearchedText != null;

        private string _filterText;

        /// <summary>
        /// The text the filtering component will filter by.
        /// </summary>
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

        /// <summary>
        /// Constructs a new <see cref="MainWindowViewModel"/> object and initializes its data adapter with the provided
        /// connection string.
        /// </summary>
        /// <param name="connectionString">The connection string to the Chami datastore.</param>
        public MainWindowViewModel(string connectionString)
        {
            _dataAdapter = new EnvironmentDataAdapter(connectionString);
            _settingsDataAdapter = new SettingsDataAdapter(connectionString);
            Environments = GetEnvironments();
            Backups = GetBackupEnvironments();
            Templates = GetTemplateEnvironments();
            EditingEnabled = false;
            if (Environments.Any())
            {
                SelectedEnvironment = ActiveEnvironment ?? Environments.First();
            }

            Settings = SettingsUtils.GetAppSettings();
            
            //EnvironmentsViewSource = new CollectionViewSource {Source = Environments};

            FilterStrategies = new ObservableCollection<IFilterStrategy>();
            InitFilterStrategies();
            IsCaseSensitiveSearch = Settings.MainWindowBehaviourSettings.IsCaseSensitiveSearch;
            _cancellationTokenSource = new CancellationTokenSource();
            CanUserInterrupt = true;
        }

        /// <summary>
        /// Initializes the filtering strategies.
        /// </summary>
        private void InitFilterStrategies()
        {
            FilterStrategy = new EnvironmentNameFilterStrategy();
            FilterStrategies.Add(FilterStrategy);
            FilterStrategies.Add(new EnvironmentAndVariableNameFilterStrategy());
            FilterStrategies.Add(new EnvironmentAndVariableNameAndValueFilterStrategy());
        }

        private bool _isDescendingSorting;

        /// <summary>
        /// Determines if the filtering/sorting component should sort its results in a descending fashion.
        /// </summary>
        public bool IsDescendingSorting
        {
            get => _isDescendingSorting;
            set
            {
                _isDescendingSorting = value;
                OnPropertyChanged(nameof(IsDescendingSorting));
            }
        }

        private bool _isCaseSensitiveSearch;

        /// <summary>
        /// Determines if the filtering component should work in a case-sensitive fashion or not.
        /// </summary>
        public bool IsCaseSensitiveSearch
        {
            get => _isCaseSensitiveSearch;
            set
            {
                _isCaseSensitiveSearch = value;

                var converter = new BooleanToStringComparisonConverter();
                var stringComparisonObject =
                    converter.ConvertBack(value, typeof(StringComparison), null, CultureInfo.CurrentUICulture);

                if (stringComparisonObject is StringComparison stringComparison)
                {
                    if (FilterStrategy != null)
                    {
                        FilterStrategy.Comparison = stringComparison;
                    }

                    OnPropertyChanged(nameof(FilterStrategy));
                }

                OnPropertyChanged(nameof(IsCaseSensitiveSearch));
            }
        }

        /// <summary>
        /// Event that gets fired when the active environment changes.
        /// </summary>
        public event EventHandler<EnvironmentChangedEventArgs> EnvironmentChanged;

        /// <summary>
        /// Determines if the Apply button in the window is enabled (i.e., there's no editing and no environment
        /// switching on progress.
        /// </summary>
        public bool ExecuteButtonPlayEnabled =>
            !EditingEnabled && CanUserInterrupt && SelectedEnvironmentTypeTabIndex == 0;

        private readonly SettingsDataAdapter _settingsDataAdapter;

        private int _selectedEnvironmentTypeTabIndex;

        public int SelectedEnvironmentTypeTabIndex
        {
            get => _selectedEnvironmentTypeTabIndex;
            set
            {
                _selectedEnvironmentTypeTabIndex = value;
                if (_selectedEnvironmentTypeTabIndex == TABITEM_NORMAL_ENV_IDX)
                {
                    SelectedEnvironment = Environments.FirstOrDefault();
                }
                else if (_selectedEnvironmentTypeTabIndex == TABITEM_BACKUP_ENV_IDX)
                {
                    SelectedEnvironment = Backups.FirstOrDefault();
                }
                else if (_selectedEnvironmentTypeTabIndex == TABITEM_TEMPLATE_ENV_IDX)
                {
                    SelectedEnvironment = Templates.FirstOrDefault();
                }

                OnPropertyChanged(nameof(SelectedEnvironmentTypeTabIndex));
                OnPropertyChanged(nameof(ExecuteButtonPlayEnabled));
            }
        }
        
        private const int TABITEM_NORMAL_ENV_IDX = 0;
        private const int TABITEM_BACKUP_ENV_IDX = 2;
        private const int TABITEM_TEMPLATE_ENV_IDX = 1;

        /// <summary>
        /// Reacts to the EnvironmentChanged event.
        /// </summary>
        /// <param name="sender">The object that generated the event.</param>
        /// <param name="args">Information about the environment change.</param>
        private void OnEnvironmentChanged(object sender, EnvironmentChangedEventArgs args)
        {
            if (args != null)
            {
                ActiveEnvironment = args.NewActiveEnvironment;
                if (ActiveEnvironment != null)
                {
                    SelectedEnvironment = ActiveEnvironment;
                }
            }
            else
            {
                ActiveEnvironment = null;
            }

            ChangeActiveEnvironment();
            EnvironmentChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Performs the act of changing the active environment to update the window appearance.
        /// </summary>
        private void ChangeActiveEnvironment()
        {
            foreach (var environment in Environments)
            {
                environment.IsActive = ActiveEnvironment != null && ActiveEnvironment.Name == environment.Name;
            }
        }

        private bool _isChangeInProgress;
        public bool IsChangeInProgress => _isChangeInProgress;

        private void SetIsChangeInProgress(bool value)
        {
            _isChangeInProgress = value;
            OnPropertyChanged(nameof(ExecuteButtonPlayEnabled));
            OnPropertyChanged(nameof(ExecuteButtonIcon));
        }

        /// <summary>
        /// Determines if all the environment variables in the <see cref="SelectedEnvironment"/> passed validation.
        /// </summary>
        /// <returns>True if all environment variables passed validation, otherwise false.</returns>
        public bool AreSelectedEnvironmentVariablesValid()
        {
            return SelectedEnvironment.EnvironmentVariables.All(envVar =>
                envVar.IsValid == null || envVar.IsValid == true);
        }

        private CancellationToken GetNewCancellationToken()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            return _cancellationTokenSource.Token;
        }

        /// <summary>
        /// Performs an environment change.
        /// </summary>
        /// <param name="progress">Reports progress notification.</param>
        public async Task ChangeEnvironmentAsync(Action<CmdExecutorProgress> progress)
        {
            SetIsChangeInProgress(true);
            var isSafetyEnabled = ((App) (Application.Current)).Settings.SafeVariableSettings.EnableSafeVars;
            var safeVariableSettings = ((App) (Application.Current)).Settings.SafeVariableSettings;
            
            var cmdExecutor = new CmdExecutor(SelectedEnvironment);
            
            cmdExecutor.EnvironmentChanged += OnEnvironmentChanged;
            cmdExecutor.SetProgressHandler(progress);
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
                        IShellCommand newCommand;
                        var isCurrentVariableDisabled =
                            safeVariableSettings.ForbiddenVariables.FirstOrDefault(v =>
                                v.Name == environmentVariable.Name) != null;
                        if (isCurrentVariableDisabled && isSafetyEnabled)
                        {
                            newCommand =
                                EnvironmentVariableCommandFactory.GetCommand(typeof(NopCommand),
                                    environmentVariable);
                        }
                        else
                        {
                            newCommand =
                                EnvironmentVariableCommandFactory.GetCommand(typeof(EnvironmentVariableRemovalCommand),
                                    environmentVariable);
                        }
                        
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
                IShellCommand newCommand;
                var isCurrentVariableDisabled =
                    safeVariableSettings.ForbiddenVariables.FirstOrDefault(v =>
                        v.Name == environmentVariable.Name) != null;
                if (isCurrentVariableDisabled && isSafetyEnabled)
                {
                    newCommand =
                        EnvironmentVariableCommandFactory.GetCommand(typeof(NopCommand), environmentVariable);
                }
                else
                {
                    newCommand = EnvironmentVariableCommandFactory.GetCommand(
                        typeof(EnvironmentVariableApplicationCommand),
                        environmentVariable);
                }

                cmdExecutor.AddCommand(newCommand);
            }

            await cmdExecutor.ExecuteAsync(cancellationToken);
            SetIsChangeInProgress(false);
        }

        /// <summary>
        /// Binding for the tooltip of the filter strategy combobox.
        /// </summary>
        public string FilterStrategyComboboxToolTip => FilterStrategy.Name;

        private CancellationTokenSource _cancellationTokenSource;

        private EnvironmentViewModel _activeEnvironment;

        /// <summary>
        /// The currently-active environment on the system.
        /// </summary>
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

        /// <summary>
        /// Synchronous version of <see cref="ChangeEnvironment"/>. Chami doesn't make use of it.
        /// </summary>
        /// <param name="progress"></param>
        public void ChangeEnvironment(Action<CmdExecutorProgress> progress)
        {
            ChangeEnvironmentAsync(progress).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Contains all environments present in the datastore.
        /// </summary>
        public ObservableCollection<EnvironmentViewModel> Environments { get; }

        /// <summary>
        /// Contains all template environments to be shown in the main window.
        /// </summary>
        public ObservableCollection<EnvironmentViewModel> Templates { get; }

        /// <summary>
        /// Contains all backup environments to be shown in the main window.
        /// </summary>
        public ObservableCollection<EnvironmentViewModel> Backups { get; }

        public void ChangeTab(EnvironmentType type)
        {
            switch (type)
            {
                case EnvironmentType.TemplateEnvironment:
                    SelectedEnvironment = Templates.FirstOrDefault();
                    SelectedEnvironmentTypeTabIndex = 1;
                    break;
                case EnvironmentType.BackupEnvironment:
                    SelectedEnvironment = Backups.FirstOrDefault();
                    SelectedEnvironmentTypeTabIndex = 2;
                    break;
                default:
                    SelectedEnvironment = Environments.FirstOrDefault();
                    SelectedEnvironmentTypeTabIndex = 0;
                    break;
            }

            SelectedVariable = SelectedEnvironment?.EnvironmentVariables.FirstOrDefault();
        }

        private EnvironmentViewModel _selectedEnvironment;

        /// <summary>
        /// The environment selected in the listbox.
        /// </summary>
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

        /// <summary>
        /// The path to the icon to show in the Execute button.
        /// If no environment is selected the play_disabled image is shown.
        /// If <see cref="ExecuteButtonPlayEnabled"/> is true, the play image is shown.
        /// Otherwise, the stop icon is shown.
        /// </summary>
        public string ExecuteButtonIcon
        {
            get
            {
                if ((SelectedEnvironment == null || (SelectedEnvironment != null && EditingEnabled) ||
                     !CanUserInterrupt) || SelectedEnvironmentTypeTabIndex != 0)
                {
                    return "/Assets/Svg/play_disabled.svg";
                }

                if (ExecuteButtonPlayEnabled && !_isChangeInProgress)
                {
                    return "/Assets/Svg/play.svg";
                }


                return "/Assets/Svg/stop.svg";
            }
        }

        /// <summary>
        /// The path to the image used in the clear filter button.
        /// </summary>
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

        /// <summary>
        /// The currently-selected variable in the listview.
        /// </summary>
        public EnvironmentVariableViewModel SelectedVariable
        {
            get => _selectedVariable;
            set
            {
                _selectedVariable = value;
                OnPropertyChanged(nameof(SelectedVariable));
                OnPropertyChanged(nameof(SelectedEnvironment.EnvironmentVariables));
                OnPropertyChanged(nameof(SelectedVariableIsFolder));
            }
        }

        public bool? SelectedVariableIsFolder => SelectedVariable.IsFolder;

        private EnvironmentVariableViewModel _selectedVariable;

        private ObservableCollection<EnvironmentViewModel> GetEnvironments()
        {
            return new ObservableCollection<EnvironmentViewModel>(_dataAdapter.GetEnvironments());
        }

        private ObservableCollection<EnvironmentViewModel> GetBackupEnvironments()
        {
            return new ObservableCollection<EnvironmentViewModel>(_dataAdapter.GetBackupEnvironments());
        }

        private ObservableCollection<EnvironmentViewModel> GetTemplateEnvironments()
        {
            return new ObservableCollection<EnvironmentViewModel>(_dataAdapter.GetTemplateEnvironments());
        }

        private readonly EnvironmentDataAdapter _dataAdapter;

        public event EventHandler<ApplicationsDetectedEventArgs> ApplicationsDetected;

        /// <summary>
        /// Constructs the message to show in the messagebox that appears when a running application is detected after
        /// changing the environment.
        /// </summary>
        /// <returns>The content of the messagebox.</returns>
        public void DetectApplicationsAndShowWindow()
        {
            var watchedApplicationSettings = Settings.WatchedApplicationSettings;
            if (watchedApplicationSettings.IsDetectionEnabled)
            {
                var applicationDetector =
                    new RunningApplicationDetector(watchedApplicationSettings.WatchedApplications);
                var detectedApplications = applicationDetector.Detect();
                if (detectedApplications is {Count: > 0})
                {
                    var window = new DetectedApplicationsWindow
                    {
                        Owner = Application.Current.MainWindow,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    ApplicationsDetected += window.OnApplicationsDetected;
                    window.Show();
                    ApplicationsDetected?.Invoke(this, new ApplicationsDetectedEventArgs(detectedApplications));
                }
            }
        }

        /// <summary>
        /// Deletes the selected environment from the datastore.
        /// </summary>
        public void DeleteSelectedEnvironment()
        {
            _dataAdapter.DeleteEnvironment(SelectedEnvironment);
            Environments.Remove(SelectedEnvironment);
            Backups.Remove(SelectedEnvironment);
            Templates.Remove(SelectedEnvironment);
            SelectedEnvironment = null;
        }

        /// <summary>
        /// Saves the currently-edited environment.
        /// </summary>
        public void SaveCurrentEnvironment()
        {
            var environment = _dataAdapter.SaveEnvironment(SelectedEnvironment);
            SelectedEnvironment = environment;
            OnPropertyChanged(nameof(Environments));
            OnPropertyChanged(nameof(SelectedEnvironment));
            DisableEditing();
        }

        /// <summary>
        /// Event that gets triggered when an environment already exists.
        /// </summary>
        public event EventHandler<EnvironmentExistingEventArgs> EnvironmentExists;

        private string _windowTitle = "Chami";

        /// <summary>
        /// The title of the main window. If no environment is active, it defaults to the application name. If there is
        /// an active environment, it shows the name of the application and that of the environment.
        /// </summary>
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

        /// <summary>
        /// Checks if an environment already exists in the environment list.
        /// </summary>
        /// <param name="environment">The environment to check for.</param>
        /// <returns>True if the environment exists in the collection, otherwise null.</returns>
        public bool CheckEnvironmentExists(EnvironmentViewModel environment)
        {
            var collection = Environments;
            if (environment.EnvironmentType == EnvironmentType.BackupEnvironment)
            {
                collection = Backups;
            }

            if (environment.EnvironmentType == EnvironmentType.TemplateEnvironment)
            {
                collection = Templates;
            }
            
            if (collection.Any(e => e.Name == environment.Name))
            {
                EnvironmentExists?.Invoke(this, new EnvironmentExistingEventArgs(environment.Name));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Executes a backup of the current environment variables.
        /// </summary>
        /// <seealso cref="EnvironmentBackupper"/>
        public void BackupEnvironment()
        {
            _dataAdapter.BackupEnvironment();
            Backups.Clear();
            var backups = GetBackupEnvironments();
            foreach (var backup in backups)
            {
                Backups.Add(backup);
            }
        }

        /// <summary>
        /// Deletes an environment variable from the datastore.
        /// </summary>
        public void DeleteSelectedVariable()
        {
            SelectedVariable.MarkForDeletion();
        }

        /// <summary>
        /// Marks the specified <seealso cref="EnvironmentVariableViewModel"/> for deletion.
        /// </summary>
        /// <param name="variableViewModel"></param>
        public void DeleteVariable(EnvironmentVariableViewModel variableViewModel)
        {
            variableViewModel.MarkForDeletion();
        }

        /// <summary>
        /// Removes all Chami environment variables from the current environment.
        /// </summary>
        /// <param name="progress">Used for progress reporting.</param>
        /// <param name="cancellationToken">Enabled canceling the task.</param>
        public async Task ResetEnvironmentAsync(Action<CmdExecutorProgress> progress,
            CancellationToken cancellationToken)
        {
            var cmdExecutor = new CmdExecutor();
            cmdExecutor.SetProgressHandler(progress);

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
                }
            }
            else
            {
                NopCommand nopCommand = new NopCommand(ChamiUIStrings.RevertToOriginalEnvironmentNop);
                cmdExecutor.AddCommand(nopCommand);
            }

            await cmdExecutor.ExecuteAsync(cancellationToken);

            OnEnvironmentChanged(this, new EnvironmentChangedEventArgs(null));
        }

        internal EnvironmentVariableViewModel CreateEnvironmentVariable()
        {
            var newVariable = new EnvironmentVariableViewModel {Environment = SelectedEnvironment};
            SelectedVariable = newVariable;
            //SelectedEnvironment.EnvironmentVariables.Add(newVariable);
            return newVariable;
        }

        /// <summary>
        /// Detects the currently-active Chami environment by querying the Windows Registry.
        /// </summary>
        public void DetectCurrentEnvironment()
        {
            var detector = new EnvironmentVariableRegistryRetriever();

            var currentEnvironmentName = detector.GetEnvironmentVariable("_CHAMI_ENV");
            OnEnvironmentChanged(this,
                new EnvironmentChangedEventArgs(Environments.FirstOrDefault(e => e.Name == currentEnvironmentName)));
        }

        /// <summary>
        /// Re-synchronizes the datastore and the UI explicitly.
        /// </summary>
        public void ResetCurrentEnvironmentFromDatasource()
        {
            if (SelectedEnvironment != null)
            {
                SelectedEnvironment = _dataAdapter.GetEnvironmentById(SelectedEnvironment.Id);
            }
        }

        private void AddEnvironmentToCorrectCollection(EnvironmentViewModel environmentViewModel)
        {
            switch (environmentViewModel.EnvironmentType)
            {
                case EnvironmentType.NormalEnvironment:
                default:
                    Environments.Add(environmentViewModel);
                    break;
                case EnvironmentType.BackupEnvironment:
                    Backups.Add(environmentViewModel);
                    break;
                case EnvironmentType.TemplateEnvironment:
                    Templates.Add(environmentViewModel);
                    break;
            }
        }


        /// <summary>
        /// Renames an environment.
        /// </summary>
        /// <param name="argsNewName">The new name of the environment.</param>
        /// <param name="progress">Reports progress.</param>
        public async Task RenameEnvironment(string argsNewName, Action<CmdExecutorProgress> progress)
        {
            var environmentToSave = SelectedEnvironment;
            environmentToSave.Name = argsNewName;
            var newSelectedEnvironment = _dataAdapter.SaveEnvironment(environmentToSave);
            Environments.Remove(SelectedEnvironment);
            Backups.Remove(SelectedEnvironment);
            Templates.Remove(SelectedEnvironment);
            AddEnvironmentToCorrectCollection(newSelectedEnvironment);

            SelectedEnvironment = newSelectedEnvironment;
            if (SelectedEnvironment.Equals(ActiveEnvironment))
            {
                ActiveEnvironment = newSelectedEnvironment;
                CanUserInterrupt = false;
                await ChangeEnvironmentAsync(progress);
            }

            SelectedEnvironment.Name = argsNewName;
            CanUserInterrupt = true;
        }

        /// <summary>
        /// Imports environments from a list of files.
        /// </summary>
        /// <param name="dialogFileNames">An array containing the paths to the files to import.</param>
        /// <returns>A <see cref="List{T}"/> of converted <see cref="EnvironmentViewModel"/>.</returns>
        public List<EnvironmentViewModel> StartImportFiles(string[] dialogFileNames)
        {
            List<EnvironmentViewModel> output = new List<EnvironmentViewModel>();
            var rejectedFiles = new List<string>();
            foreach (var fileName in dialogFileNames)
            {
                try
                {
                    var reader = EnvironmentReaderFactory.GetEnvironmentReaderByExtension(fileName);
                    try
                    {
                        var viewModel = reader.Process();
                        output.Add(viewModel);
                    }
                    catch (JsonReaderException)
                    {
                        var viewModels = reader.ProcessMultiple();
                        foreach (var model in viewModels)
                        {
                            output.Add(model);
                        }
                    }
                }
                catch (NotSupportedException)
                {
                    rejectedFiles.Add(fileName + "\n");
                }
            }

            if (rejectedFiles.Any())
            {
                MessageBox.Show(string.Concat(rejectedFiles), "Some files were rejected!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return output;
        }

        private bool _canUserInterrupt;

        public bool CanUserInterrupt
        {
            get => _canUserInterrupt;
            set
            {
                _canUserInterrupt = value;
                OnPropertyChanged(nameof(CanUserInterrupt));
                OnPropertyChanged(nameof(ExecuteButtonPlayEnabled));
                OnPropertyChanged(nameof(ExecuteButtonIcon));
            }
        }

        /// <summary>
        /// Saves the state of the main window to the datastore.
        /// </summary>
        /// <param name="width">The width of the window.</param>
        /// <param name="height">The height of the window.</param>
        /// <param name="xPosition">The position of the top left corner of the window on the screen on the X coordinate.</param>
        /// <param name="yPosition">The position of the top left corner of the window on the screen on the Y coordinate.</param>
        /// <param name="windowState">The current state of the main window.</param>
        /// <param name="sortDescription">The sorting used by the listview.</param>
        /// <seealso cref="MainWindowSavedBehaviourViewModel"/>
        /// <seealso cref="SettingsDataAdapter"/>
        public void SaveWindowState(double width, double height, double xPosition, double yPosition,
            WindowState windowState,
            SortDescription sortDescription)
        {
            var settings = Settings.MainWindowBehaviourSettings;
            settings.IsCaseSensitiveSearch = IsCaseSensitiveSearch;
            settings.Height = height;
            settings.Width = width;
            settings.XPosition = xPosition;
            settings.YPosition = yPosition;
            settings.SearchPath = FilterStrategy;
            settings.SortDescription = sortDescription;
            
            // Avoid starting minimized on next application start.
            if (windowState == WindowState.Minimized)
            {
                windowState = WindowState.Normal;
            }

            settings.WindowState = windowState;
            _settingsDataAdapter.SaveMainWindowState(Settings);
        }

        public async Task<IEnumerable<EnvironmentVariableBlacklistViewModel>> SaveBlacklistedVariables(
            IEnumerable<EnvironmentVariableBlacklistViewModel> blacklistedVariables)
        {
            var tasks = new List<Task<EnvironmentVariableBlacklistViewModel>>();
            var output = new List<EnvironmentVariableBlacklistViewModel>();
            foreach (var variable in blacklistedVariables)
            {
                Task<EnvironmentVariableBlacklistViewModel> task = _dataAdapter.SaveBlacklistedVariableAsync(variable);
                tasks.Add(task);
                task.ContinueWith(async v =>
                {
                    var awaitedVariable = await v;
                    output.Add(awaitedVariable);
                });
            }

            await Task.WhenAll(tasks);
            return output;
        }

        /// <summary>
        /// Opens the folder pointed by the <see cref="SelectedVariable"/>.
        /// </summary>
        /// <exception cref="ChamiFolderException">If the folder doesn't exist, an exception is thrown.</exception>
        public void OpenFolder()
        {
            // We need to call the Replace method because explorer.exe doesn't treat / as a directory separator and opens the Documents folder instead.
            var folderPath = System.Environment.ExpandEnvironmentVariables(SelectedVariable.Value).Replace("/", "\\");
            if (Directory.Exists(folderPath))
            {
                var openInExplorerCommand = new OpenInExplorerCommand(folderPath);
                openInExplorerCommand.Execute();
            }
            else
            {
                throw new ChamiFolderException(ChamiUIStrings.UnableToOpenAsFolderMessage);
            }
        }

        public bool IsSelectedVariableDeletable()
        {
            return SelectedEnvironment?.EnvironmentType != EnvironmentType.BackupEnvironment;
        }
    }
}