using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Windows.AdvancedExportWindow
{
    public partial class AdvancedExportWindow : Window
    {
        public AdvancedExportWindow()
        {
            InitializeComponent();
        }

        private void EnvironmentComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataContext = DataContext as AdvancedExportWindowViewModel;
            if (e.RemovedItems.Count == 0)
            {
                return;
            }
            
            if (dataContext == null)
            {
                return;
            }
            
            var environment = e.RemovedItems[0] as EnvironmentViewModel;
            dataContext.ClearMarkedVariables(environment);
        }
    }
}