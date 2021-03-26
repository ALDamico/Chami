using System.Windows.Controls;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Controls
{
    public partial class SafeVariableEditor : UserControl
    {
        public SafeVariableEditor()
        {
            _viewModel = new SafeVariableViewModel();
            DataContext = _viewModel;
            InitializeComponent();
        }

        private SafeVariableViewModel _viewModel;

        public SafeVariableEditor(SafeVariableViewModel settingsSafeVariableSettings)
        {
            _viewModel = new SafeVariableViewModel();
            DataContext = _viewModel;
            InitializeComponent();
        }
    }
}