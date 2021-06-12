using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ChamiUI.Localization;

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
            return true;
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public event EventHandler<EnvironmentSavedEventArgs> EnvironmentSaved;

        private void NewEnvironmentWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            EnvironmentNameTextbox.Focus();
        }

        private void SaveCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var inserted = _viewModel.SaveEnvironment();
            if (!inserted)
            {
                MessageBox.Show(ChamiUIStrings.UnableToInsertEnvironmentMessageBoxText,
                    ChamiUIStrings.UnableToInsertEnvironmentMessageBoxCaption, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                var insertedEnvironment = _viewModel.GetInsertedEnvironment();
                EnvironmentSaved?.Invoke(this, new EnvironmentSavedEventArgs(insertedEnvironment));
            }

            Close();
        }

        private void NewEnvironmentWindowSaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_viewModel.IsSaveButtonEnabled &&
                _viewModel.Environment.EnvironmentVariables.All(v => v.IsValid == null || v.IsValid == true)
                && !string.IsNullOrWhiteSpace(_viewModel.EnvironmentName)
            )
            {
                _viewModel.SaveEnvironment();
                Close();
            }
            else
            {
                MessageBox.Show(ChamiUIStrings.ValidationFailedMessageBoxText,
                    ChamiUIStrings.ValidationFailedMessageBoxCaption);
            }
        }
    }
}