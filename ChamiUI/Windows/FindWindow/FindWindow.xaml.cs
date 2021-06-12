using System.Windows;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Windows.FindWindow
{
    public partial class FindWindow : Window
    {
        public FindWindow()
        {
            _viewModel = new FindWindowViewModel();
            DataContext = _viewModel;
            InitializeComponent();
        }

        private FindWindowViewModel _viewModel;
    }
}