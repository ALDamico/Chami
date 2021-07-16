using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Controls
{
    /// <summary>
    /// Control for managing safety. Currently unused and non functional.
    /// </summary>
    public partial class SafeVariableEditor
    {
        private readonly SafeVariableViewModel _settingsSafeVariableSettings;

        public SafeVariableEditor()
        {
            var viewModel = new SafeVariableViewModel();
            DataContext = viewModel;
            InitializeComponent();
        }

        public SafeVariableEditor(SafeVariableViewModel settingsSafeVariableSettings)
        {
            _settingsSafeVariableSettings = settingsSafeVariableSettings;
            var viewModel = new SafeVariableViewModel();
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}