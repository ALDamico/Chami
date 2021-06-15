using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Windows.ImportEnvironmentWindow
{
    public partial class ImportEnvironmentWindow : Window
    {
        public ImportEnvironmentWindow()
        {
            _viewModel = new ImportEnvironmentWindowViewModel();
            DataContext = _viewModel;
            InitializeComponent();
        }

        private ImportEnvironmentWindowViewModel _viewModel;
        
        

        public void SetEnvironments(IEnumerable<EnvironmentViewModel> viewModels)
        {
            foreach (var viewModel in viewModels)
            {
                _viewModel.NewEnvironments.Add(viewModel);
            }
        }
        /*public NewEnvironmentWindow()
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

        private readonly NewEnvironmentViewModel _viewModel;*/

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

/*
        private void SaveCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var inserted = _viewModel.SaveEnvironment();
            if (inserted == null)
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
        }*/

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
                    else
                    {
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