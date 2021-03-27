using System;
using ChamiUI.PresentationLayer.Events;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class SettingsViewModel: ViewModelBase
    {
        public SettingsViewModel()
        {
            LoggingSettings = new LoggingSettingsViewModel();
            SafeVariableSettings = new SafeVariableViewModel();
            ConsoleAppearanceSettings = new ConsoleAppearanceViewModel();
        }
        public LoggingSettingsViewModel LoggingSettings { get; set; }
        public SafeVariableViewModel SafeVariableSettings { get; set; }
        public ConsoleAppearanceViewModel ConsoleAppearanceSettings { get; set; }
        
    }
}