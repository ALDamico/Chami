using System.Collections.Generic;
using ChamiUI.BusinessLayer.Adapters;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ChamiUI.BusinessLayer.Converters;
using ChamiUI.BusinessLayer.EnvironmentHealth;
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
        public SettingsWindowViewModel(SettingsDataAdapter settingsDataAdapter, WatchedApplicationDataAdapter watchedApplicationDataAdapter, EnvironmentHealthChecker healthChecker)
        {
            _dataAdapter = settingsDataAdapter;
            _watchedApplicationDataAdapter = watchedApplicationDataAdapter;
            _healthChecker = healthChecker;
            SettingsCategories = new ObservableCollection<GenericLabelViewModel>();
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

            Settings.CategoriesSettings = SettingsCategoriesFactory.GetCategoriesSettingsViewModel(Settings);
            SettingsCategories.Add(Settings.CategoriesSettings);

            CurrentSection = SettingsCategories.FirstOrDefault();
        }

        private EnvironmentHealthChecker _healthChecker;

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

            var columnInfosToUpdate = Settings.HealthCheckSettings.ColumnInfoViewModels;
            _dataAdapter.SaveColumnInfoAsync(columnInfosToUpdate).GetAwaiter().GetResult();
            var converter = new ColumnInfoConverter();
            Settings.HealthCheckSettings.ColumnInfos.Clear();

            foreach (var columnInfoViewModel in columnInfosToUpdate)
            {
                Settings.HealthCheckSettings.ColumnInfos.Add(converter.From(columnInfoViewModel));
            }

            if (!Settings.HealthCheckSettings.IsEnabled)
            {
                _healthChecker.DisableCheck();
            }
        }

        private GenericLabelViewModel _currentSection;

        public GenericLabelViewModel CurrentSection
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

        public ObservableCollection<GenericLabelViewModel> SettingsCategories { get; }

        public async Task SaveForbiddenVariables()
        {
            var tasks = new List<Task<EnvironmentVariableBlacklistViewModel>>();
            var output = new List<EnvironmentVariableBlacklistViewModel>();
            foreach (var variable in Settings.SafeVariableSettings.ForbiddenVariables)
            {
                Task<EnvironmentVariableBlacklistViewModel> task = _dataAdapter.SaveBlacklistedVariableAsync(variable);
                tasks.Add(task);

                await Task.WhenAll(tasks);
            }

            output.AddRange(tasks.Select(task => task.Result));
        }

        private readonly SettingsDataAdapter _dataAdapter;
        private readonly WatchedApplicationDataAdapter _watchedApplicationDataAdapter;

        /// <summary>
        /// The entire application settings.
        /// </summary>
        public SettingsViewModel Settings => (Application.Current as App)?.Settings;
    }
}