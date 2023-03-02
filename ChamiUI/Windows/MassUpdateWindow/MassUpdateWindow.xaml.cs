using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;
using ChamiUI.Windows.Abstract;

namespace ChamiUI.Windows.MassUpdateWindow
{
    public partial class MassUpdateWindow : ChamiWindow
    {
        public MassUpdateWindow(MassUpdateWindowViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private readonly MassUpdateWindowViewModel _viewModel;

        private async void MassUpdateWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadDataAsync();
        }

        private void StrategyComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MassUpdateStrategyViewModel oldValue = null;
            MassUpdateStrategyViewModel newValue = null;

            if (e.RemovedItems.Count > 0)
            {
                oldValue = e.RemovedItems[0] as MassUpdateStrategyViewModel;
            }

            if (e.AddedItems.Count > 0)
            {
                newValue = e.AddedItems[0] as MassUpdateStrategyViewModel;
            }

            if (oldValue != null && newValue != null)
            {
                if (!newValue.CreateIfNotExistsEnabled) return;
                if (!oldValue.CreateIfNotExistsEnabled)
                {
                    return;
                }

                newValue.CreateIfNotExists = oldValue.CreateIfNotExists;
            }
        }
    }
}