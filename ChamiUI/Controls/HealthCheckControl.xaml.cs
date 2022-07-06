using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Chami.Db.Entities;
using ChamiUI.PresentationLayer.Utils;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Controls
{
    public partial class HealthCheckControl : UserControl
    {
        public HealthCheckControl()
        {
            InitializeComponent();
            var successful =
                Resources.TryGetCollectionViewSource("ColumnsCollectionViewSource", out var collectionViewSource);
            if (successful)
            {
                collectionViewSource.SortDescriptions.Add(new SortDescription(nameof(ColumnInfoViewModel.OrdinalPosition), ListSortDirection.Ascending));
            }
        }

        private void HealthCheckControl_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = e.NewValue as HealthCheckSettingsViewModel;

            viewModel?.InitColumnInfoViewModels();
        }

        private void MoveColumnUpButton_OnClick(object sender, RoutedEventArgs e)
        {
            var viewModel = GetViewModel();

            viewModel.MoveSelectedColumnUp();
        }
        
        private HealthCheckSettingsViewModel GetViewModel()
        {
            return DataContext as HealthCheckSettingsViewModel;
        }

        private void ToggleVisibilityButton_OnClick(object sender, RoutedEventArgs e)
        {
            var viewModel = GetViewModel();
            viewModel.ToggleCurrentColumnVisibility();
        }

        private void MoveColumnDownButton_OnClick(object sender, RoutedEventArgs e)
        {
            var viewModel = GetViewModel();
            viewModel.MoveSelectedColumnDown();
        }

        private void ColumnInfoListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = GetViewModel();
            viewModel.UpdateSelection(e.AddedItems, e.RemovedItems);
        }
    }
}