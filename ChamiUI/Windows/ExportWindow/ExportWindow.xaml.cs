using ChamiUI.PresentationLayer.ViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChamiUI.Windows.ExportWindow
{
    /// <summary>
    /// Logica di interazione per ExportWindow.xaml
    /// </summary>
    public partial class ExportWindow 
    {
        public ExportWindow(Window owner, ICollection<EnvironmentViewModel> environments)
        {
            Owner = owner;
            _viewModel = new ExportWindowViewModel(environments);
            
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
