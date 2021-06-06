using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
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

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
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

        public event EventHandler<EnvironmentSavedEventArgs> EnvironmentSaved;

        private void NewEnvironmentWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            EnvironmentNameTextbox.Focus();
        }
    }
}