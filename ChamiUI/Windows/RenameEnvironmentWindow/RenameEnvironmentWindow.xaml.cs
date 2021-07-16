using System;
using System.Windows;
using System.Windows.Input;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Windows.RenameEnvironmentWindow
{
    public partial class RenameEnvironmentWindow
    {
        private readonly RenameEnvironmentViewModel _viewModel;
        public RenameEnvironmentWindow()
        {
            _viewModel = new RenameEnvironmentViewModel();
            DataContext = _viewModel;
            InitializeComponent();
        }

        public RenameEnvironmentWindow(Window owner, string initialName)
        {
            Owner = owner;
            _viewModel = new RenameEnvironmentViewModel(initialName);
            DataContext = _viewModel;
            InitializeComponent();
        }

        public event EventHandler<EnvironmentRenamedEventArgs> EnvironmentRenamed;

        protected virtual void OnEnvironmentRenamed(string newName)
        {
            EnvironmentRenamed?.Invoke(this, new EnvironmentRenamedEventArgs(newName));
        }
        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_viewModel.IsNameValid)
            {
                OnEnvironmentRenamed(_viewModel.Name);
            }

            Close();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DoRename_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = false;
            if (_viewModel.IsNameValid)
            {
                e.CanExecute = true;
            }
        }

        private void DoRename_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            OkButton_OnClick(sender, e);
        }

        private void CancelRename_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = true;
        }

        private void CancelRename_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }
}