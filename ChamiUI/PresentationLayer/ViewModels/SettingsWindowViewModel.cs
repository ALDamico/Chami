using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.BusinessLayer.Factories;
using ChamiUI.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using ChamiUI.Localization;

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
            _languageDataAdapter = new ApplicationLanguageDataAdapter(connectionString);
            Settings = SettingsViewModelFactory.GetSettings(_dataAdapter, _watchedApplicationDataAdapter,
                _languageDataAdapter);
            _controls = new Dictionary<string, UserControl>();

            var viewKey = ChamiUIStrings.ViewCategory;
            _controls[viewKey] = new ConsoleAppearanceEditor(Settings.ConsoleAppearanceSettings);
            var loggingKey = ChamiUIStrings.LoggingCategory;
            _controls[loggingKey] = new LoggingSettingsEditor(Settings.LoggingSettings);
            var safetyKey = ChamiUIStrings.SafetyCategory;
            _controls[safetyKey] = new SafeVariableEditor(Settings.SafeVariableSettings);
            var detectorKey = ChamiUIStrings.DetectorCategory;
            _controls[detectorKey] = new ApplicationDetectorControl(Settings.WatchedApplicationSettings);
            var languageKey = ChamiUIStrings.LanguageCategory;
            _controls[languageKey] = new LanguageSelectorControl(Settings.LanguageSettings);
            DisplayedControl = _controls.Values.FirstOrDefault();
            var minimizationKey = ChamiUIStrings.MinimizationCategory;
            _controls[minimizationKey] = new MinimizationBehaviourControl(Settings.MinimizationBehaviour);
        }

        /// <summary>
        /// Saves the changes to the settings to the datastore.
        /// </summary>
        public void SaveSettings()
        {
            _dataAdapter.SaveSettings(Settings);
            _watchedApplicationDataAdapter.SaveWatchedApplications(Settings.WatchedApplicationSettings
                .WatchedApplications);
        }

        private SettingsDataAdapter _dataAdapter;
        private WatchedApplicationDataAdapter _watchedApplicationDataAdapter;
        private ApplicationLanguageDataAdapter _languageDataAdapter;
        private Dictionary<string, UserControl> _controls;

        /// <summary>
        /// Changes the displayed control when the user clicks on a different entry in the side bar.
        /// </summary>
        /// <param name="name">The name of the control to get.</param>
        public void ChangeControl(string name)
        {
            var controlExists = _controls.TryGetValue(name, out var control);
            if (controlExists)
            {
                DisplayedControl = control;
            }
            else
            {
                DisplayedControl = null;
            }
        }

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
    }
}