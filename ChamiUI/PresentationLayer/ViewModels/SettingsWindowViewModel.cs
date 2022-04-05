using System.Collections.Generic;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.BusinessLayer.Factories;
using ChamiUI.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
            Settings.SafeVariableSettings = SettingsCategoriesFactory.GetSafeVariableSettingCategory(Settings);
            SettingsCategories.Add(Settings.SafeVariableSettings);
            Settings.WatchedApplicationSettings =
                SettingsCategoriesFactory.GetWatchedApplicationsSettingCategory(Settings);
            SettingsCategories.Add(Settings.WatchedApplicationSettings);
            Settings.LanguageSettings = SettingsCategoriesFactory.GetLanguageSettingCategory(Settings);
            SettingsCategories.Add(Settings.LanguageSettings);
            Settings.MinimizationBehaviour =
                SettingsCategoriesFactory.GetMinimizationBehaviourSettingCategory(Settings);
            SettingsCategories.Add(Settings.MinimizationBehaviour);
            Settings.HealthCheckSettings = SettingsCategoriesFactory.GetHealthCheckSettingCategory(Settings);
            SettingsCategories.Add(Settings.HealthCheckSettings);

            CurrentSection = SettingsCategories.FirstOrDefault();
        }

        /// <summary>
        /// Saves the changes to the settings to the datastore.
        /// </summary>
        public void SaveSettings()
        {
            _dataAdapter.SaveSettings(Settings);
            var savedVariables = _dataAdapter.SaveBlacklistedVariableListAsync(Settings.SafeVariableSettings.ForbiddenVariables).GetAwaiter()
                .GetResult();
            
            Settings.SafeVariableSettings.ForbiddenVariables.Clear();

            foreach (var variable in savedVariables)
            {
                Settings.SafeVariableSettings.ForbiddenVariables.Add(variable);
            }
            Settings.WatchedApplicationSettings
                .WatchedApplications = new ObservableCollection<WatchedApplicationViewModel>(
                _watchedApplicationDataAdapter.SaveWatchedApplications(Settings.WatchedApplicationSettings
                    .WatchedApplications));
        }

        private SettingCategoryViewModelBase _currentSection;

        public SettingCategoryViewModelBase CurrentSection
        {
            get => _currentSection;
            set
            {
                if (value != null)
                {
                    _currentSection = value;
                }

                OnPropertyChanged(nameof(CurrentSection));
            }
        }

        public ObservableCollection<SettingCategoryViewModelBase> SettingsCategories { get; }

        public async Task SaveForbiddenVariables()
        {
            var tasks = new List<Task<EnvironmentVariableBlacklistViewModel>>();
            var output = new List<EnvironmentVariableBlacklistViewModel>();
            foreach (var variable in Settings.SafeVariableSettings.ForbiddenVariables)
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
        }

        private readonly SettingsDataAdapter _dataAdapter;
        private readonly WatchedApplicationDataAdapter _watchedApplicationDataAdapter;

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