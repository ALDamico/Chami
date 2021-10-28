using System.Windows;
using System.Windows.Controls;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Controls.ContentTemplateSelectors
{
    public class MainWindowContentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ConsoleDataTemplate { get; set; }
        public DataTemplate VariablesDataTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ConsoleTabViewModel)
            {
                return ConsoleDataTemplate;
            }

            if (item is VariablesDataGridViewModel)
            {
                return VariablesDataTemplate;
            }
            return base.SelectTemplate(item, container);
        }
    }
}