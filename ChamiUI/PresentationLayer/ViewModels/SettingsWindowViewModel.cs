using System;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.BusinessLayer.Factories;
using ChamiUI.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            AvailableControls = new ObservableCollection<ControlKeyWrapper>();

            var viewKey = ChamiUIStrings.ViewCategory;
            var viewKeyWrapper = new ControlKeyWrapper(viewKey, new ConsoleAppearanceEditor(Settings.ConsoleAppearanceSettings));
            AvailableControls.Add(viewKeyWrapper);
            
            var loggingKey = ChamiUIStrings.LoggingCategory;
            var loggingKeyWrapper = new ControlKeyWrapper(loggingKey, new LoggingSettingsEditor(Settings.LoggingSettings));
            AvailableControls.Add(loggingKeyWrapper);
            var safetyKey = ChamiUIStrings.SafetyCategory;
            var safetyKeyWrapper = new ControlKeyWrapper(safetyKey, new SafeVariableEditor(Settings.SafeVariableSettings));
            AvailableControls.Add(safetyKeyWrapper);
            var detectorKey = ChamiUIStrings.DetectorCategory;
            var detectorKeyWrapper = new ControlKeyWrapper(detectorKey, new ApplicationDetectorControl(Settings.WatchedApplicationSettings));
            AvailableControls.Add(detectorKeyWrapper);
            var languageKey = ChamiUIStrings.LanguageCategory;
            var languageKeyWrapper = new ControlKeyWrapper(languageKey, new LanguageSelectorControl(Settings.LanguageSettings));
            AvailableControls.Add(languageKeyWrapper);
            var minimizationKey = ChamiUIStrings.MinimizationCategory;
            var minimizationKeyWrapper = new ControlKeyWrapper(minimizationKey, new MinimizationBehaviourControl(Settings.MinimizationBehaviour));
            DisplayedControl = AvailableControls.FirstOrDefault()?.Control;
            
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
        
        public ObservableCollection<ControlKeyWrapper> AvailableControls { get; }

        private SettingsDataAdapter _dataAdapter;
        private WatchedApplicationDataAdapter _watchedApplicationDataAdapter;
        private ApplicationLanguageDataAdapter _languageDataAdapter;
        

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