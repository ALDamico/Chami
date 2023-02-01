using ChamiUI.PresentationLayer.ViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace ChamiUI.Windows.ExportWindow
{
    /// <summary>
    /// Logica di interazione per ExportWindow.xaml
    /// </summary>
    public partial class ExportWindow 
    {
        public ExportWindow(ICollection<EnvironmentViewModel> environments)
        {
            _viewModel = new ExportWindowViewModel(AppUtils.GetAppServiceProvider().GetRequiredService<EnvironmentDataAdapter>(), environments);
            
            DataContext = _viewModel;
            InitializeComponent();
            
        }

        private ExportWindowViewModel _viewModel;

        private void CancelCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CancelCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void SelectedEnvironmentListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.HandleSelectionChanged(sender, e);
        }

        private async void ExportDataButton_OnClick(object sender, RoutedEventArgs e)
        {
           await  _viewModel.ExportAsync();
        }
    }
}
