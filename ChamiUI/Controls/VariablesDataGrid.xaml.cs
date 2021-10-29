using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ChamiUI.BusinessLayer.Exceptions;
using ChamiUI.PresentationLayer.ViewModels;
using ChamiUI.PresentationLayer.ViewModels.Interfaces;

namespace ChamiUI.Controls
{
    public partial class VariablesDataGrid : UserControl
    {
        public VariablesDataGrid(IEnvironmentDatagridModel viewModel)
        {
            DataContext = viewModel;
            ViewModel = viewModel;
            InitializeComponent();
        }

        private IEnvironmentDatagridModel ViewModel { get; set; }

        private void CopyEnvironmentVariableMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(ViewModel.SelectedVariable.Value);
        }

        private void OpenAsFolderMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.OpenFolder();
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
                        ViewModel.SelectedVariable = environmentVariableViewModel;
                        ViewModel.DeleteSelectedVariable();

                        e.Handled = true;
                    }
                }
            }
        }

        private void DeleteEnvironmentVariableMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedEnvironmentVariables = new List<object>();
            foreach (var envVar in CurrentEnvironmentVariablesDataGrid.SelectedItems)
            {
                selectedEnvironmentVariables.Add(envVar);
            }

            foreach (var environmentVariable in selectedEnvironmentVariables)
            {
                if (environmentVariable is EnvironmentVariableViewModel vm)
                {
                    ViewModel.SelectedVariable = vm;
                    ViewModel.DeleteSelectedVariable();
                }
            }
        }
    }
}