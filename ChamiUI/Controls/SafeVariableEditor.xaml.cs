using System.Windows;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Controls
{
    /// <summary>
    /// Control for managing safety. Currently unused and non functional.
    /// </summary>
    public partial class SafeVariableEditor
    {
        public SafeVariableEditor()
        {
            InitializeComponent();
        }

        private async void SafeVariableEditor_OnLoaded(object sender, RoutedEventArgs e)
        {
            await GetDataContextAsSafeVariableViewModel().LoadForbiddenVariables();
        }

        public SafeVariableViewModel GetDataContextAsSafeVariableViewModel()
        {
            return DataContext as SafeVariableViewModel;
        }
    }
}