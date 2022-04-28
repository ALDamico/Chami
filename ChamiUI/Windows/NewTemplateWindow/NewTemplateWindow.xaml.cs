using System;
using System.Data.SQLite;
using System.Windows;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.Utils;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Windows.NewTemplateWindow
{
    public partial class NewTemplateWindow
    {
        public NewTemplateWindow(Window owner)
        {
            Owner = owner;
            _viewmodel = new NewTemplateWindowViewModel();
            DataContext = _viewmodel;
            InitializeComponent();
        }

        private readonly NewTemplateWindowViewModel _viewmodel;

        public event EventHandler<EnvironmentSavedEventArgs> EnvironmentSaved; 

        private void NewEnvironmentWindowSaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewmodel.SaveTemplate();
                EnvironmentSaved?.Invoke(this, new EnvironmentSavedEventArgs(_viewmodel.Environment));
                Close();
            }
            catch (SQLiteException ex)
            {
                var loggingEnabled = SettingsUtils.GetAppSettings().LoggingSettings.LoggingEnabled;
                if (loggingEnabled)
                {
                    var logger = (App.Current as ChamiUI.App).GetLogger();
                    logger.Error("{Message}", ex.Message);
                    logger.Error("{StackTrace}", ex.StackTrace);
                }

                string message = "";
                string caption = "";
                    
                if (ex.ErrorCode == (int) SQLiteErrorCode.Constraint_Unique)
                {
                    message = string.Format(ChamiUIStrings.SaveEnvironmentErrorMessage, _viewmodel.TemplateName);
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

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}