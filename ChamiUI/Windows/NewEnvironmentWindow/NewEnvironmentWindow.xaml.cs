using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ChamiUI.Localization;

namespace ChamiUI.Windows.NewEnvironmentWindow
{
    public partial class NewEnvironmentWindow
    {
        public NewEnvironmentWindow()
        {
            _viewModel = new NewEnvironmentViewModel();
            DataContext = _viewModel;
            InitializeComponent();
        }

        public void SetEnvironment(EnvironmentViewModel environmentViewModel)
        {
            _viewModel.Environment = environmentViewModel;
            _viewModel.EnvironmentName = environmentViewModel.Name;
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

        private void NewEnvironmentWindowSaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_viewModel.IsSaveButtonEnabled &&
                _viewModel.Environment.EnvironmentVariables.All(v => v.IsValid == null || v.IsValid == true)
                && !string.IsNullOrWhiteSpace(_viewModel.EnvironmentName)
            )
            {
                var inserted = _viewModel.SaveEnvironment();
                EnvironmentSaved?.Invoke(this, new EnvironmentSavedEventArgs(inserted));
                Close();
            }
            else
            {
                MessageBox.Show(ChamiUIStrings.ValidationFailedMessageBoxText,
                    ChamiUIStrings.ValidationFailedMessageBoxCaption);
            }
        }

        private void TemplateEnvironmentCombobox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnvironmentViewModel old = null;
            EnvironmentViewModel newEnv = null;
            if (e.AddedItems.Count > 0)
            {
                newEnv = e.AddedItems[0] as EnvironmentViewModel;
            }

            _viewModel.ChangeTemplate(newEnv.Name);
        }
    }
}