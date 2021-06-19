using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Windows.ImportEnvironmentWindow
{
    public partial class ImportEnvironmentWindow
    {
        public ImportEnvironmentWindow()
        {
            _viewModel = new ImportEnvironmentWindowViewModel();
            DataContext = _viewModel;
            InitializeComponent();
        }

        private readonly ImportEnvironmentWindowViewModel _viewModel;

        public void SetEnvironments(IEnumerable<EnvironmentViewModel> viewModels)
        {
            foreach (var viewModel in viewModels)
            {
                _viewModel.NewEnvironments.Add(viewModel);
            }

            _viewModel.SelectedEnvironment ??= _viewModel.NewEnvironments[0];
        }

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

        private void ImportEnvironmentWindowSaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            bool canSave = true;
            if (_viewModel.IsSaveButtonEnabled)
            {
                foreach (var environment in _viewModel.NewEnvironments)
                {
                    if (!(environment.EnvironmentVariables.All(v => v.IsValid == null || v.IsValid == true) &&
                          !string.IsNullOrWhiteSpace(environment.Name)))
                    {
                        canSave = false;
                    }
                }

                if (canSave)
                {
                    var inserted = _viewModel.SaveEnvironments();
                    
                    foreach (var environment in inserted)
                    {
                        EnvironmentSaved?.Invoke(this, new EnvironmentSavedEventArgs(environment));                        
                    }
                }
                else
                {
                    // TODO use a more descriptive message
                    MessageBox.Show(ChamiUIStrings.ValidationFailedMessageBoxText,
                        ChamiUIStrings.ValidationFailedMessageBoxCaption);
                }

                Close();
            }
        }
    }
}