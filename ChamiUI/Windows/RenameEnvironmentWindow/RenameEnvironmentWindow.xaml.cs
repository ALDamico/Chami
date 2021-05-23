using System;
using System.Windows;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Windows.RenameEnvironmentWindow
{
    public partial class RenameEnvironmentWindow : Window
    {
        private RenameEnvironmentViewModel _viewModel;
        public RenameEnvironmentWindow()
        {
            _viewModel = new RenameEnvironmentViewModel();
            DataContext = _viewModel;
            InitializeComponent();
        }

        public RenameEnvironmentWindow(string initialName)
        {
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
    }
}