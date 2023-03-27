using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// Represents all the configurable aspects of the Chami application and enables the application to react to those
    /// configurable aspects.
    /// </summary>
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel(MinimizationBehaviourViewModel minimizationBehaviourViewModel)
        {
            LoggingSettings = new LoggingSettingsViewModel();
            SafeVariableSettings = new SafeVariableViewModel(AppUtils.GetAppServiceProvider().GetRequiredService<EnvironmentDataAdapter>());
            ConsoleAppearanceSettings = new ConsoleAppearanceViewModel();
            WatchedApplicationSettings = new WatchedApplicationControlViewModel();
            LanguageSettings = new LanguageSelectorViewModel();
            MainWindowBehaviourSettings = new MainWindowSavedBehaviourViewModel();
            MinimizationBehaviour = minimizationBehaviourViewModel;
            HealthCheckSettings = new HealthCheckSettingsViewModel();
        }
        /// <summary>
        /// Contains all the settings related to logging.
        /// </summary>
        public LoggingSettingsViewModel LoggingSettings { get; set; }
        /// <summary>
        /// Contains the settings that protects the system from Chami disrupting some critical environment variables
        /// like PATH.
        /// Currently not implemented.
        /// </summary>
        public SafeVariableViewModel SafeVariableSettings { get; set; }
        /// <summary>
        /// Contains the settings related to the appearance of the console component inside the main window.
        /// </summary>
        public ConsoleAppearanceViewModel ConsoleAppearanceSettings { get; set; }
        /// <summary>
        /// Contains the settings related to the applications Chami will scan and notify the user if any are running, so
        /// that the user can manually restart them to force a change in the environment.
        /// </summary>
        public WatchedApplicationControlViewModel WatchedApplicationSettings { get; set; }
        /// <summary>
        /// Contains the settings related to localization.
        /// </summary>
        public LanguageSelectorViewModel LanguageSettings { get; set; }
        /// <summary>
        /// Contains the settings related to saving and resuming the state of the main application window, so that when
        /// the application is restarted or minimized to tray and restored, those changes won't be lost.
        /// </summary>
        public MainWindowSavedBehaviourViewModel MainWindowBehaviourSettings { get; set; }
        /// <summary>
        /// Contains the settings related to how the main window should react when the user clicks the minimize button.
        /// </summary>
        public MinimizationBehaviourViewModel MinimizationBehaviour { get; set; }
        public HealthCheckSettingsViewModel HealthCheckSettings { get; set; }
        public CategoriesSettingsViewModel CategoriesSettings { get; set; }
    }
}