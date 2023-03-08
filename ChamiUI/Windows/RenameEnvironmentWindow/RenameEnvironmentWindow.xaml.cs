using System;
using System.Windows;
using System.Windows.Input;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Windows.RenameEnvironmentWindow
{
    public partial class RenameEnvironmentWindow
    {

        public RenameEnvironmentWindow(RenameEnvironmentViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}