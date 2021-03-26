using System.Windows;
using System.Windows.Controls;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Windows.SettingsWindow
{
    public partial class SettingsWindow : Window
    {
        private readonly SettingsWindowViewModel _settingsWindowViewModel;
        public SettingsWindow()
        {
            _settingsWindowViewModel = new SettingsWindowViewModel();
            DataContext = _settingsWindowViewModel;
            InitializeComponent();
        }

        public SettingsWindow(SettingsViewModel viewModel)
        {
            _settingsWindowViewModel = new SettingsWindowViewModel(viewModel);
            DataContext = _settingsWindowViewModel;
            InitializeComponent();
        }

        private void CategoriesTree_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var name = (e.NewValue as TreeViewItem)?.Header.ToString();
            _settingsWindowViewModel.ChangeControl(name);
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}