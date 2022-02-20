using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;
using System;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.Utils;

namespace ChamiUI.Windows.NewEnvironmentWindow
{
    public partial class NewEnvironmentWindow
    {
        public NewEnvironmentWindow(Window owner) : this()
        {
            Owner = owner;
        }

        public void SetEnvironment(EnvironmentViewModel environmentViewModel)
        {
            _viewModel.Environment = environmentViewModel;
            _viewModel.EnvironmentName = environmentViewModel.Name;
        }

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

        private void NewEnvironmentWindowSaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_viewModel.IsSaveButtonEnabled &&
                _viewModel.Environment.EnvironmentVariables.All(v => v.IsValid == null || v.IsValid == true)
                && !string.IsNullOrWhiteSpace(_viewModel.EnvironmentName)
               )
            {
                try
                {
                    var inserted = _viewModel.SaveEnvironment();
                    EnvironmentSaved?.Invoke(this, new EnvironmentSavedEventArgs(inserted));
                    Close();
                }
                catch (SQLiteException ex)
                {
                    var loggingEnabled = SettingsUtils.GetAppSettings().LoggingSettings.LoggingEnabled;
                    if (loggingEnabled)
                    {
                        var logger = (App.Current as ChamiUI.App).GetLogger();
                        logger.Error(ex.Message);
                        logger.Error(ex.StackTrace);
                    }

                    string message = "";
                    string caption = "";
                    
                    if (ex.ErrorCode == (int) SQLiteErrorCode.Constraint_Unique)
                    {
                        message = string.Format(ChamiUIStrings.SaveEnvironmentErrorMessage, _viewModel.EnvironmentName);
                        caption = ChamiUIStrings.SaveEnvironmentErrorCaption;
                    }
                    else
                    {
                        message = string.Format(ChamiUIStrings.SaveEnvironmentUnknownErrorMessage, ex.Message,
                            ex.StackTrace);
                        caption = ChamiUIStrings.SaveEnvironmentUnknownErrorCaption;
                    }
                    MessageBox.Show(message, caption, MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show(ChamiUIStrings.ValidationFailedMessageBoxText,
                    ChamiUIStrings.ValidationFailedMessageBoxCaption);
            }
        }

        private void TemplateEnvironmentCombobox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.ChangeTemplate();
        }
    }
}