using System.Windows;
using System.Windows.Controls;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.PresentationLayer.DataTemplateSelectors
{
    public class SettingsWindowDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LoggingDataTemplate { get; set; }
        public DataTemplate ConsoleAppearanceDataTemplate { get; set; }
        public DataTemplate SafeVariableDataTemplate { get; set; }
        public DataTemplate WatchedApplicationsDataTemplate { get; set; }
        public DataTemplate LanguagesDataTemplate { get; set; }
        public DataTemplate MinimizationBehaviourDataTemplate { get; set; }
        public DataTemplate AdvancedExportersDataTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is LoggingSettingsViewModel)
            {
                return LoggingDataTemplate;
            }

            if (item is ConsoleAppearanceViewModel)
            {
                return ConsoleAppearanceDataTemplate;
            }

            if (item is SafeVariableViewModel)
            {
                return SafeVariableDataTemplate;
            }

            if (item is WatchedApplicationControlViewModel)
            {
                return WatchedApplicationsDataTemplate;
            }

            if (item is LanguageSelectorViewModel)
            {
                return LanguagesDataTemplate;
            }

            if (item is MinimizationBehaviourViewModel)
            {
                return MinimizationBehaviourDataTemplate;
            }

            if (item is AdvancedExporterSettingsViewModel)
            {
                return AdvancedExportersDataTemplate;
            }
            
            return base.SelectTemplate(item, container);
        }
    }
}