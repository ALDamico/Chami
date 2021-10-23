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
            //Loads data only if this is the first time the control is being loaded
            if (DataContext is not SafeVariableViewModel dataContext) return;
            if (dataContext.ForbiddenVariables.Count == 0)
            {
                await dataContext.LoadForbiddenVariables();
            }
        }
    }
}