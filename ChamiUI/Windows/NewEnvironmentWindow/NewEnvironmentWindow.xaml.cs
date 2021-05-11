using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;

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

        protected override void OnClosing(CancelEventArgs e)
        {

            e.Cancel = !HandleClosing();
            base.OnClosing(e);
        }

        private bool HandleClosing()
        {
            /*if (_viewModel.DetectChanges())
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
                    return true;
                }
            }
            else
            {
                return true;
            }

            return false;*/
            return true;
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
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

        private void NewEnvironmentWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            EnvironmentNameTextbox.Focus();
        }
    }
}