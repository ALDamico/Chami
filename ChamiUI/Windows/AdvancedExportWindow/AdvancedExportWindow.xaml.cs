using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ChamiUI.PresentationLayer.Utils;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Windows.AdvancedExportWindow
{
    public partial class AdvancedExportWindow
    {
        public AdvancedExportWindow()
        {
            InitializeComponent();
            
            var settings = SettingsUtils.GetAppSettings();
            
            SetColumnWidthOrDefault(0, settings.AdvancedExporterSettings.VariableNameColumnWidth);
            SetColumnWidthOrDefault(1, settings.AdvancedExporterSettings.IsMarkedColumnWidth);
        }

        private void SetColumnWidthOrDefault(int index, double value)
        {
            if (index >= AdvancedExportWindowVariablesGridView.Columns.Count)
            {
                return;
            }
            if (value == 0)
            {
                value = 250;
            }
            AdvancedExportWindowVariablesGridView.Columns[index].Width = value;
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

            if (vm == null)
            {
                MessageBox.Show("Error generating preview!");
                return;
            }
            
            vm.GeneratePreview();

            var previewWindow = new PreviewWindow
            {
                Owner = this,
                DataContext = vm
            };

            previewWindow.Show();
        }

        private void AdvancedExportWindow_OnClosing(object sender, CancelEventArgs e)
        {
            var viewModel = DataContext as AdvancedExportWindowViewModel;

            if (viewModel == null)
            {
                return;
            }
            
            var setting = SettingsUtils.GetAppSettings().AdvancedExporterSettings;

            if (setting != null)
            {
                setting.SessionMaxLineLength = viewModel.LineMaxLength;
                setting.VariableNameColumnWidth = AdvancedExportWindowVariablesGridView.Columns[0].Width;
                setting.IsMarkedColumnWidth = AdvancedExportWindowVariablesGridView.Columns[1].Width;
            }
        }
    }
}