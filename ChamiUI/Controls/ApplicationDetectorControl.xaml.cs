using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using ChamiUI.PresentationLayer.ViewModels;
using System.Windows.Controls;
using System.Windows.Documents;
using ChamiUI.BusinessLayer;
using ChamiUI.BusinessLayer.Commands;
using ChamiUI.Localization;
using Serilog;

namespace ChamiUI.Controls
{
    /// <summary>
    ///  This is the control in the Settings window that enables the user to detect if some application is running so
    /// that they can restart them manually.
    /// </summary>
    public partial class ApplicationDetectorControl
    {
        /// <summary>
        /// Constructs a new <see cref="ApplicationDetectorControl"/>,
        /// </summary>
        /// <param name="viewModel">The viewModel containing window state.</param>
        public ApplicationDetectorControl()
        {
            InitializeComponent();
        }

        private WatchedApplicationControlViewModel GetDataContextAsWatchedApplicationViewModel()
        {
            return DataContext as WatchedApplicationControlViewModel;
        }

        private void WatchedApplicationsDataGrid_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Delete)
            {
                var datagrid = (DataGrid) sender;
                if (datagrid != null)
                {
                    var selectedRows = datagrid.SelectedItems;
                    if (selectedRows.Count == 0)
                    {
                        return;
                    }

                    foreach (var row in selectedRows)
                    {
                        var viewModel = row as WatchedApplicationViewModel;
                        GetDataContextAsWatchedApplicationViewModel().DeleteWatchedApplication(viewModel);
                        e.Handled = true;
                    }
                }
            }
        }

        private void HyperLinkClickHandler(object sender, RoutedEventArgs e)
        {
            var destination = ((Hyperlink) e.OriginalSource).NavigateUri?.ToString().Replace("file:///", "");
            if (destination != null)
            {
                var viewModel = GetDataContextAsWatchedApplicationViewModel();
                try
                {
                    viewModel.RunApplication(destination);
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(string.Format(ChamiUIStrings.RunApplicationFileNotFoundErrorMessage, destination),
                        ChamiUIStrings.RunApplicationFileNotFoundErrorCaption,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    Log.Logger.Error(ex, "{Ex}", ex);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        string.Format(ChamiUIStrings.UnknownErrorWhenRunningApplicationErrorMessage, destination),
                        ChamiUIStrings.UnknownErrorWhenRunningApplicationErrorCaption,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    Log.Logger.Error(ex, "{Ex}", ex);
                }
            }
            else
            {
                MessageBox.Show("Impossibile eseguire l'applicazione", "Informazione", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
    }
}