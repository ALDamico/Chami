using ChamiUI.PresentationLayer.ViewModels;
using System.Windows.Controls;

namespace ChamiUI.Controls
{
    /// <summary>
    /// Control for managing safety. Currently unused and non functional.
    /// </summary>
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