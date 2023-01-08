using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ChamiUI.PresentationLayer.Filtering;
using ChamiUI.PresentationLayer.Utils;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Controls;

public partial class MainWindowFilterControl : UserControl
{
    public MainWindowFilterControl()
    {
        InitializeComponent();
    }
    
    internal void FocusFilterTextboxCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        FilterTextbox.Focus();
    }
    
    public MainWindowViewModel ViewModel => DataContext as MainWindowViewModel;

    private void ClearFilterButton_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModel.FilterText = null;
    }
    
    private void CaseSensitivityCheckBox_OnChecked(object sender, RoutedEventArgs e)
    {
        RefreshEnvironmentViewSource();
    }
    
    internal void SortByIdMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        SortByIdRadioButton.IsChecked = true;
        if (ViewModel.IsDescendingSorting)
        {
            ChangeSorting(SortDescriptionUtils.SortByIdDescending);
            return;
        }

        ChangeSorting(SortDescriptionUtils.SortByIdAscending);
    }
    
    private void ChangeSorting(SortDescription sortDescription)
    {
        if (Resources["EnvironmentsViewSource"] is CollectionViewSource collectionViewSource)
        {
            collectionViewSource.SortDescriptions.Clear();
            collectionViewSource.SortDescriptions.Add(sortDescription);
            collectionViewSource.View.Refresh();
        }
    }
    
    internal void SortByNameMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        SortByNameRadioButton.IsChecked = true;
        if (ViewModel.IsDescendingSorting)
        {
            ChangeSorting(SortDescriptionUtils.SortByNameDescending);
            return;
        }

        ChangeSorting(SortDescriptionUtils.SortByNameAscending);
    }
    
    internal void SortByDateMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        SortByDateAddedRadioButton.IsChecked = true;
        if (ViewModel.IsDescendingSorting)
        {
            ChangeSorting(SortDescriptionUtils.SortByDateAddedDescending);
            return;
        }

        ChangeSorting(SortDescriptionUtils.SortByDateAddedAscending);
    }
    
    private void ToggleSortDirection()
    {
        Resources.TryGetCollectionViewSource("EnvironmentsViewSource", out var collectionViewSource);
        if (collectionViewSource != null)
        {
            var sortDescription = SortDescriptionUtils.GetOppositeSorting(collectionViewSource.SortDescriptions[0]);
            ChangeSorting(sortDescription);
        }
    }
    
    internal void SortDescendingMenuItem_OnChecked(object sender, RoutedEventArgs e)
    {
        if (sender is MenuItem)
        {
            return;
        }

        ToggleSortDirection();
    }
    
    internal void SortDescendingMenuItem_OnUnchecked(object sender, RoutedEventArgs e)
    {
        if (sender is MenuItem)
        {
            return;
        }

        ToggleSortDirection();
    }
    
    internal void InitSortDescription()
    {
        Resources.TryGetCollectionViewSource("EnvironmentsViewSource", out var collectionViewSource);
        if (collectionViewSource != null)
        {
            collectionViewSource.SortDescriptions.Add(SortDescriptionUtils.SortByIdAscending);
        }
        var sortDescription = ViewModel.Settings.MainWindowBehaviourSettings.SortDescription;
        collectionViewSource.SortDescriptions.Add(sortDescription);
        switch (sortDescription.PropertyName)
        {
            default:
                SortByIdRadioButton.IsChecked = true;
                break;
            case "Name":
                SortByNameRadioButton.IsChecked = true;
                break;
            case "AddedOn":
                SortByDateAddedRadioButton.IsChecked = true;
                break;
        }

        if (sortDescription.PropertyName == "Id")
        {
            SortByIdRadioButton.IsChecked = true;
        }
            
        ViewModel.IsDescendingSorting = sortDescription.Direction == ListSortDirection.Descending;
    }
    
    private void FilterStrategySelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var newStrategy = e.AddedItems[0] as IFilterStrategy;
        ViewModel.ChangeFilterStrategy(newStrategy);
        RefreshEnvironmentViewSource();
    }
    
    private void RefreshEnvironmentViewSource()
    {
        Resources.TryGetCollectionViewSource("EnvironmentsViewSource", out var collectionViewSource);
        if (collectionViewSource != null)
        {
            collectionViewSource.View.Refresh();
        }
    }
    
    private void FilterTextbox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        SubscribeToFilterEvent("EnvironmentsViewSource");
        SubscribeToFilterEvent("BackupEnvironmentsViewSource");
    }
    
    private void SubscribeToFilterEvent(string viewSourceName)
    {
        Resources.TryGetCollectionViewSource(viewSourceName, out var collectionViewSource);
        if (collectionViewSource != null)
        {
            collectionViewSource.Filter += ViewModel.FilterStrategy.OnFilter;
        }
    }
}