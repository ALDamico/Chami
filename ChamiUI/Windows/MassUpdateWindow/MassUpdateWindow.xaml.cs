using System.Windows;
using System.Windows.Controls;
using ChamiUI.PresentationLayer.ViewModels;
using ChamiUI.Windows.Abstract;

namespace ChamiUI.Windows.MassUpdateWindow
{
    public partial class MassUpdateWindow : ChamiWindow
    {
        public MassUpdateWindow(MassUpdateWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}