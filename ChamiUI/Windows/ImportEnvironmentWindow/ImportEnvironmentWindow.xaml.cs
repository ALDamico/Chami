using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ChamiUI.BusinessLayer.Converters;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Windows.ImportEnvironmentWindow
{
    public partial class ImportEnvironmentWindow
    {
        public ImportEnvironmentWindow(Window owner)
        {
            Owner = owner;
            _viewModel = new ImportEnvironmentWindowViewModel();
            DataContext = _viewModel;
            InitializeComponent();
        }

        private readonly ImportEnvironmentWindowViewModel _viewModel;

        public void SetEnvironments(IEnumerable<EnvironmentViewModel> viewModels)
        {
            var environmentViewModels = viewModels.ToList();
            if (!environmentViewModels.Any())
            {
                return;
            }

            var converter = new ImportEnvironmentViewModelConverter();
            foreach (var viewModel in environmentViewModels)
            {
                var importViewModel = converter.To(viewModel);
                if (importViewModel != null)
                {
                    importViewModel.ShouldImport = true;
                    _viewModel.NewEnvironments.Add(importViewModel);
                }
               
            }

            _viewModel.SelectedEnvironment ??= _viewModel.NewEnvironments[0];
            _viewModel.UpdatePropertyChanged();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = false;
            base.OnClosing(e);
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
                        var args = new EnvironmentSavedEventArgs(environment);
                        args.CheckEnvironmentExistence = false;
                        EnvironmentSaved?.Invoke(this, new EnvironmentSavedEventArgs(environment));                        
                    }
                }
                else
                {
                    MessageBox.Show(ChamiUIStrings.ValidationFailedMessageBoxText,
                        ChamiUIStrings.ValidationFailedMessageBoxCaption);
                }

                Close();
            }
        }

        private void DeleteCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_viewModel.SelectedEnvironment != null)
            {
                e.CanExecute = true;
            }
        }

        private void UpdateCheckedStatus(object sender, RoutedEventArgs e)
        {
            _viewModel.UpdatePropertyChanged();
        }

        private void ImportSelectAllButton_OnClick(object sender, RoutedEventArgs e)
        {
            _viewModel.SelectAllEnvironments();
        }

        private void ImportDeselectAllButton_OnClick(object sender, RoutedEventArgs e)
        {
            _viewModel.DeselectAllEnvironments();
        }
    }
}