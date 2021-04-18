using ChamiUI.PresentationLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChamiUI.Windows.ExportWindow
{
    /// <summary>
    /// Logica di interazione per ExportWindow.xaml
    /// </summary>
    public partial class ExportWindow : Window
    {
        public ExportWindow(ICollection<EnvironmentViewModel> environments)
        {
            _viewModel = new ExportWindowViewModel(environments);
            
            DataContext = _viewModel;
            InitializeComponent();
        }

        private ExportWindowViewModel _viewModel;

        private void CancelCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //TODO Probably will need to change this
            e.CanExecute = true;
        }

        private void CancelCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }
}
