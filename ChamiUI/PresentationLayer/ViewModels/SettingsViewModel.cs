using System.Globalization;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
            LoggingSettings = new LoggingSettingsViewModel();
            SafeVariableSettings = new SafeVariableViewModel();
            ConsoleAppearanceSettings = new ConsoleAppearanceViewModel();
            WatchedApplicationSettings = new WatchedApplicationControlViewModel();
            LanguageSettings = new LanguageSelectorViewModel();
            MainWindowBehaviourSettings = new MainWindowSavedBehaviourViewModel();
        }
        public LoggingSettingsViewModel LoggingSettings { get; set; }
        public SafeVariableViewModel SafeVariableSettings { get; set; }
        public ConsoleAppearanceViewModel ConsoleAppearanceSettings { get; set; }
        public WatchedApplicationControlViewModel WatchedApplicationSettings { get; set; }
        public LanguageSelectorViewModel LanguageSettings { get; set; }
        public MainWindowSavedBehaviourViewModel MainWindowBehaviourSettings { get; set; }
    }
}