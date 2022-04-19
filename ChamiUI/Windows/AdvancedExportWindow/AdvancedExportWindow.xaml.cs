using System.Windows;
using System.Windows.Controls;
using ChamiUI.BusinessLayer.Exporters;
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
            if (e.RemovedItems.Count == 0)
            {
                return;
            }


            if (DataContext is not AdvancedExportWindowViewModel dataContext)
            {
                return;
            }
            
            var oldEnvironment = e.RemovedItems[0] as EnvironmentViewModel;
            EnvironmentViewModel newEnvironment = null;
            if (e.AddedItems.Count > 0)
            {
                newEnvironment = e.AddedItems[0] as EnvironmentViewModel;
            }
            
            dataContext.ClearMarkedVariables(oldEnvironment, newEnvironment);
        }

        private void AdvancedExportWindowSelectAllButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is not AdvancedExportWindowViewModel viewModel)
            {
                return;
            }

            viewModel.SetAllVariables(viewModel.SelectedEnvironment, true);
        }

        private void AdvancedExportWindowDeselectAllButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is not AdvancedExportWindowViewModel viewModel)
            {
                return;
            }

            viewModel.SetAllVariables(viewModel.SelectedEnvironment, false);
        }

        private void AdvancedExportWindowCloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AdvancedExportWindowPreviewButton_OnClick(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as AdvancedExportWindowViewModel;
            var exportInfo = new ScriptExportInfo()
            {
                Environment = vm.SelectedEnvironment,
                MaxLineLength = vm.LineMaxLength,
                Remarks = vm.Remarks
            };

            var exporter = new EnvironmentBatchFileExporter(exportInfo);
            var preview = exporter.GetPreview("abcd");

            MessageBox.Show(preview);
        }
    }
}