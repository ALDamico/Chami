using ChamiUI.PresentationLayer.ViewModels;
using System.Windows.Controls;

namespace ChamiUI.Controls
{
    public partial class ConsoleAppearanceEditor : UserControl
    {
        public ConsoleAppearanceEditor()
        {
            _viewModel = new ConsoleAppearanceViewModel();
            DataContext = _viewModel;
            InitializeComponent();
        }

        private ConsoleAppearanceViewModel _viewModel;
    }
}