using System.Windows;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Windows.MassUpdateWindow
{
    public partial class MassUpdateWindow : Window
    {
        public MassUpdateWindow()
        {
            InitializeComponent();
            _viewModel = new MassUpdateWindowViewModel();
            DataContext = _viewModel;
        }

        private MassUpdateWindowViewModel _viewModel;
    }
}