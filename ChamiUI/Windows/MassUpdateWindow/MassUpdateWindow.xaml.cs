using System;
using System.Windows;
using System.Windows.Controls;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Windows.MassUpdateWindow
{
    public partial class MassUpdateWindow : Window
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
/*
        private void EnvironmentsListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.HandleSelectionChanged(e.AddedItems, e.RemovedItems);
        }*/
/* TODO Move this to service
        private void CloseCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var prompt = MessageBox.Show(ChamiUIStrings.ConfirmCloseWindowText,
                ChamiUIStrings.ConfirmCloseWindowCaption, MessageBoxButton.YesNo, MessageBoxImage.Question,
                MessageBoxResult.No);
            if (prompt == MessageBoxResult.Yes)
            {
                Close();
            }
        }


        private void ExecuteCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (_viewModel.ShouldShowWarningMessageBox())
            {
                var choice = MessageBox.Show(this, string.Format(ChamiUIStrings
                        .ConfirmMassUpdateWithEmptyValueMessageBoxMessage, _viewModel.VariableToUpdate),
                    ChamiUIStrings.ConfirmMassUpdateWithEmptyValueMessageBoxCaption, MessageBoxButton.YesNo,
                    MessageBoxImage.Warning, MessageBoxResult.No);
                if (choice == MessageBoxResult.No)
                {
                    return;
                }
            }
            _viewModel.ExecuteUpdate().GetAwaiter().GetResult();
            OnMassUpdateExecuted(new MassUpdateEventArgs());
        }
*/

        private void MassUpdateWindowSelectAllButton_OnClick(object sender, RoutedEventArgs e)
        {
            _viewModel.SelectAllEnvironments();
        }

        private void MassUpdateWindowSelectNoneButton_OnClick(object sender, RoutedEventArgs e)
        {
            _viewModel.DeselectAllEnvironments();
        }

        public event EventHandler<MassUpdateEventArgs> MassUpdateExecuted;

        protected virtual void OnMassUpdateExecuted(MassUpdateEventArgs e)
        {
            MassUpdateExecuted?.Invoke(this, e);
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