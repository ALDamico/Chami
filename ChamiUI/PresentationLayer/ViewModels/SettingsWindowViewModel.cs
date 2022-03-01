using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.BusinessLayer.Factories;
using ChamiUI.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.Factories;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// Viewmodel class for the settings window.
    /// </summary>
    public class SettingsWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Constructs a new <see cref="SettingsWindowViewModel"/> object and initializes its <see cref="Settings"/>
        /// property and its data adapters.
        /// </summary>
        public SettingsWindowViewModel()
        {
            var connectionString = App.GetConnectionString();
            _dataAdapter = new SettingsDataAdapter(connectionString);
            _watchedApplicationDataAdapter = new WatchedApplicationDataAdapter(connectionString);
            var languageDataAdapter = new ApplicationLanguageDataAdapter(connectionString);
            Settings = SettingsViewModelFactory.GetSettings(_dataAdapter, _watchedApplicationDataAdapter,
                languageDataAdapter);
            SettingsCategories = new ObservableCollection<SettingCategoryViewModelBase>();
            Settings.ConsoleAppearanceSettings =
                SettingsCategoriesFactory.GetConsoleAppearanceCategory(Settings);
            SettingsCategories.Add(Settings.ConsoleAppearanceSettings);
            Settings.LoggingSettings = SettingsCategoriesFactory.GetLoggingSettingCategory(Settings);
            SettingsCategories.Add(Settings.LoggingSettings);
            SettingsCategories.Add(Settings.SafeVariableSettings);
            SettingsCategories.Add(Settings.WatchedApplicationSettings);
            Settings.LanguageSettings = SettingsCategoriesFactory.GetLanguageSettingCategory(Settings);
            SettingsCategories.Add(Settings.LanguageSettings);
            SettingsCategories.Add(Settings.MinimizationBehaviour);
            
            AvailableControls = new ObservableCollection<ControlKeyWrapper>();

          

            
            var safetyKey = ChamiUIStrings.SafetyCategory;
            var safetyKeyWrapper = new ControlKeyWrapper(safetyKey, new SafeVariableEditor(Settings.SafeVariableSettings));
            AvailableControls.Add(safetyKeyWrapper);
            var detectorKey = ChamiUIStrings.DetectorCategory;
            var detectorKeyWrapper = new ControlKeyWrapper(detectorKey, new ApplicationDetectorControl(Settings.WatchedApplicationSettings));

            
            AvailableControls.Add(detectorKeyWrapper);
            var languageKey = ChamiUIStrings.LanguageCategory;
            //var languageKeyWrapper = new ControlKeyWrapper(languageKey, new LanguageSelectorControl(Settings.LanguageSettings));
            //AvailableControls.Add(languageKeyWrapper);
            var minimizationKey = ChamiUIStrings.MinimizationCategory;
            var minimizationKeyWrapper = new ControlKeyWrapper(minimizationKey, new MinimizationBehaviourControl(Settings.MinimizationBehaviour));
            AvailableControls.Add(minimizationKeyWrapper);
            DisplayedControl = AvailableControls.FirstOrDefault()?.Control;
        }

        /// <summary>
        /// Saves the changes to the settings to the datastore.
        /// </summary>
        public void SaveSettings()
        {
            _dataAdapter.SaveSettings(Settings);
            Settings.WatchedApplicationSettings
                .WatchedApplications = new ObservableCollection<WatchedApplicationViewModel>(_watchedApplicationDataAdapter.SaveWatchedApplications(Settings.WatchedApplicationSettings
                .WatchedApplications));
        }

        private SettingCategoryViewModelBase _currentSection;

        public SettingCategoryViewModelBase CurrentSection
        {
            get => _currentSection;
            set
            {
                _currentSection = value;
                OnPropertyChanged(nameof(CurrentSection));
            }
        }

        public ObservableCollection<ControlKeyWrapper> AvailableControls { get; }
        public ObservableCollection<SettingCategoryViewModelBase> SettingsCategories { get; }

        private readonly SettingsDataAdapter _dataAdapter;
        private readonly WatchedApplicationDataAdapter _watchedApplicationDataAdapter;


        private UserControl _displayedControl;

        /// <summary>
        /// The currently-displayed control.
        /// </summary>
        public UserControl DisplayedControl
        {
            get => _displayedControl;
            set
            {
                _displayedControl = value;
                OnPropertyChanged(nameof(DisplayedControl));
            }
        }

        private SettingsViewModel _settingsViewModel;

        /// <summary>
        /// The entire application settings.
        /// </summary>
        public SettingsViewModel Settings
        {
            get => _settingsViewModel;
            set
            {
                _settingsViewModel = value;
                OnPropertyChanged(nameof(Settings));
            }
        }

        /// <summary>
        /// Changes the displayed control
        /// </summary>
        /// <param name="controlKey">The key to use to find the control to set.</param>
        public void ChangeControl(ControlKeyWrapper controlKey)
        {
            DisplayedControl = AvailableControls.FirstOrDefault(c => c.Guid.Equals(controlKey.Guid))?.Control;
        }
    }
}