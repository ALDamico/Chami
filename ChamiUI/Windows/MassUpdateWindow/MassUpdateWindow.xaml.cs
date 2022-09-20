using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ChamiUI.BusinessLayer.MassUpdateStrategies;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Windows.MassUpdateWindow
{
    public partial class MassUpdateWindow : Window
    {
        public MassUpdateWindow()
        {
            InitializeComponent();
            _viewModel = new MassUpdateWindowViewModel();
            DataContext = _viewModel;
        }

        private MassUpdateWindowViewModel _viewModel;

        private async void MassUpdateWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadDataAsync();
        }

        private void EnvironmentsListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.HandleSelectionChanged(e.AddedItems, e.RemovedItems);
        }

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

        private void CloseCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public static readonly RoutedCommand ExecuteCommand = new RoutedCommand();

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

        private void ExecuteCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                e.CanExecute = _viewModel.ExecuteButtonEnabled;
            }
        }

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