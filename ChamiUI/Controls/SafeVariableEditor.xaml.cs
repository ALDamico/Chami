using System.Windows;
using ChamiUI.PresentationLayer.ViewModels;
using ChamiUI.Utils;

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
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is SafeVariableViewModel viewModel)
            {
                viewModel.LoadForbiddenVariables().Await();
            }
        }
    }
}