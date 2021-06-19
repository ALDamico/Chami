using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ChamiUI.Windows.SettingsWindow
{
    public partial class SettingsWindow
    {
        private readonly SettingsWindowViewModel _settingsWindowViewModel;

        public void SaveSettings()
        {
            _settingsWindowViewModel.SaveSettings();
        }

        public SettingsWindow()
        {
            _settingsWindowViewModel = new SettingsWindowViewModel();
            DataContext = _settingsWindowViewModel;
            InitializeComponent();
        }
        public event EventHandler<SettingsSavedEventArgs> SettingsSaved;

        private void CategoriesTree_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var name = (e.NewValue as TreeViewItem)?.Header.ToString();
            _settingsWindowViewModel.ChangeControl(name);
        }

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