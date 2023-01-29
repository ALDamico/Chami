using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;
using System;
using System.Windows;

namespace ChamiUI.Windows.SettingsWindow
{
    public partial class SettingsWindow
    {
        private readonly SettingsWindowViewModel _settingsWindowViewModel;

        public SettingsWindow(SettingsWindowViewModel settingsViewModel)
        {
            InitializeComponent();
            //Owner = owner;
            _settingsWindowViewModel = settingsViewModel;
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