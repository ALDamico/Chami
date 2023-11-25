using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;
using System;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.Utils;
using Serilog;

namespace ChamiUI.Windows.NewEnvironmentWindow
{
    public partial class NewEnvironmentWindow
    {
        public NewEnvironmentWindow(Window owner, NewEnvironmentViewModel environmentViewModelBase)
        {
            Owner = owner;
            _viewModel = environmentViewModelBase;
            DataContext = _viewModel;
            InitializeComponent();
        }
        
        public void SetEnvironment(EnvironmentViewModel environmentViewModel)
        {
            _viewModel.Environment = environmentViewModel;
            _viewModel.EnvironmentName = environmentViewModel.Name;
        }

        private readonly NewEnvironmentViewModel _viewModel;
    }
}