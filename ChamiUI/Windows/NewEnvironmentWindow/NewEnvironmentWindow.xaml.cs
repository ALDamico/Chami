using System;
using System.Windows;
using ChamiUI.PresentationLayer;
using ChamiUI.PresentationLayer.Events;

namespace ChamiUI.Windows.NewEnvironmentWindow
{
    public partial class NewEnvironmentWindow : Window
    {
        public NewEnvironmentWindow()
        {
            _viewModel = new NewEnvironmentViewModel();
            DataContext = _viewModel;
            InitializeComponent();
        }

        private readonly NewEnvironmentViewModel _viewModel;

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_viewModel.DetectChanges())
            {
                string caption;
                string environmentName = _viewModel.Environment.Name;

                if (!string.IsNullOrWhiteSpace(environmentName))
                {
                    caption = $"The environment {environmentName} has been changed. Are you sure you want to cancel?";
                }
                else
                {
                    caption = "The environment has been changed. Are you sure you want to cancel?";
                }

                var result = MessageBox.Show(caption, "Are you sure you want to cancel?", MessageBoxButton.YesNo,
                    MessageBoxImage.Question, MessageBoxResult.No);
                if (result == MessageBoxResult.Yes)
                {
                    Close();
                }
            }
            else
            {
                Close();
            }
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var inserted = _viewModel.SaveEnvironment();
            if (!inserted)
            {
                MessageBox.Show("Unable to insert your new environment!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                EnvironmentSaved?.Invoke(this, new EnvironmentSavedEventArgs(_viewModel.Environment));
            }
            Close();
        }

        public event EventHandler<EnvironmentSavedEventArgs> EnvironmentSaved;
    }
}