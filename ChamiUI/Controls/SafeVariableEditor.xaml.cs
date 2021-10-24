using System.Windows;
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
            _settingsSafeVariableSettings = viewModel;
            InitializeComponent();
        }

        public SafeVariableEditor(SafeVariableViewModel settingsSafeVariableSettings)
        {
            _settingsSafeVariableSettings = settingsSafeVariableSettings;
            DataContext = _settingsSafeVariableSettings;
            InitializeComponent();
        }

        private async void SafeVariableEditor_OnLoaded(object sender, RoutedEventArgs e)
        {
            await _settingsSafeVariableSettings.LoadForbiddenVariables();
        }
    }
}