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
        }
        public LoggingSettingsViewModel LoggingSettings { get; set; }
        public SafeVariableViewModel SafeVariableSettings { get; set; }
        public ConsoleAppearanceViewModel ConsoleAppearanceSettings { get; set; }
        public WatchedApplicationControlViewModel WatchedApplicationSettings { get; set; }
        

    }
}