using System.Windows;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Windows.NewTemplateWindow
{
    public partial class NewTemplateWindow : Window
    {
        public NewTemplateWindow()
        {
            _viewmodel = new NewTemplateWindowViewModel();
            DataContext = _viewmodel;
            InitializeComponent();
        }

        private NewTemplateWindowViewModel _viewmodel;
        

        private void NewEnvironmentWindowSaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            _viewmodel.SaveTemplate();
            Close();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}