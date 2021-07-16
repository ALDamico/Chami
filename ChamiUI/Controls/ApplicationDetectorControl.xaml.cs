using ChamiUI.PresentationLayer.ViewModels;
using System.Windows.Controls;

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
        public ApplicationDetectorControl(WatchedApplicationControlViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = _viewModel;
            InitializeComponent();
        }

        private readonly WatchedApplicationControlViewModel _viewModel;

        private void WatchedApplicationsDataGrid_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Delete)
            {
                var datagrid = (DataGrid)sender;
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
                        _viewModel.DeleteWatchedApplication(viewModel);
                        e.Handled = true;
                    }
                }
            }
        }
    }
}