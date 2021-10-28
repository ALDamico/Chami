using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ChamiUI.BusinessLayer.Exceptions;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Controls
{
    public partial class VariablesDataGrid : UserControl
    {
        public VariablesDataGrid(ViewModelBase viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }

        private void CopyEnvironmentVariableMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;
            if (viewModel != null)
            {
                Clipboard.SetText(viewModel.SelectedVariable.Value);
            }
        }

        private void OpenAsFolderMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;
            try
            {
                viewModel.OpenFolder();
            }
            catch (ChamiFolderException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CurrentEnvironmentVariablesDataGrid_OnPreviewKeyDow(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                foreach (var row in CurrentEnvironmentVariablesDataGrid.SelectedItems)
                {
                    if (row is EnvironmentVariableViewModel environmentVariableViewModel)
                    {
                        var viewModel = DataContext as MainWindowViewModel;
                        if (viewModel != null)
                        {
                            viewModel.SelectedVariable = environmentVariableViewModel;
                            viewModel.DeleteSelectedVariable();
                        }

                        e.Handled = true;
                    }
                }
            }
        }

        private void DeleteEnvironmentVariableMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;
            var selectedEnvironmentVariables = new List<object>();
            foreach (var envVar in CurrentEnvironmentVariablesDataGrid.SelectedItems)
            {
                selectedEnvironmentVariables.Add(envVar);
            }

            foreach (var environmentVariable in selectedEnvironmentVariables)
            {
                if (environmentVariable is EnvironmentVariableViewModel vm)
                {
                    viewModel.SelectedVariable = vm;
                    viewModel.DeleteSelectedVariable();
                }
            }
        }
    }
}