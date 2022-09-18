using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using ChamiUI.BusinessLayer.Processes;
using Microsoft.Extensions.DependencyInjection;

namespace ChamiUI.Windows.SettingsWindow
{
    public partial class SettingsWindow
    {
        private readonly SettingsWindowViewModel _settingsWindowViewModel;

        public SettingsWindow(Window owner, ProcessLauncherService processLauncherService)
        {
            InitializeComponent();
            Owner = owner;
            _settingsWindowViewModel = new SettingsWindowViewModel(processLauncherService);
            DataContext = _settingsWindowViewModel;
            
        }
        public event EventHandler<SettingsSavedEventArgs> SettingsSaved;

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ApplyButton_OnClick(object sender, RoutedEventArgs e)
        {
            HandleSettingsSaved();
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            HandleSettingsSaved();
            Close();
        }

        private void HandleSettingsSaved()
        {
            _settingsWindowViewModel.SaveSettings();
            SettingsSaved?.Invoke(this, new SettingsSavedEventArgs(_settingsWindowViewModel.Settings));
        }
    }
}