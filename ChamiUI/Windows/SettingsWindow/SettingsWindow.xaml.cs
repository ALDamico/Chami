using System.Windows;
using System.Windows.Controls;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Windows.SettingsWindow
{
    public partial class SettingsWindow : Window
    {
        private readonly SettingsViewModel _settingsViewModel;
        public SettingsWindow()
        {
            _settingsViewModel = new SettingsViewModel();
            DataContext = _settingsViewModel;
            InitializeComponent();
        }

        private void CategoriesTree_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var name = (e.NewValue as TreeViewItem)?.Header.ToString();
            _settingsViewModel.ChangeControl(name);
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}