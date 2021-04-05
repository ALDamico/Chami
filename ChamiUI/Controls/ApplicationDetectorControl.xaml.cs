using ChamiUI.PresentationLayer.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ChamiUI.Controls
{
    public partial class ApplicationDetectorControl : UserControl
    {
        public ApplicationDetectorControl(WatchedApplicationControlViewModel viewModel)
        {
            _viewModel = viewModel;
            InitializeComponent();
        }

        private WatchedApplicationControlViewModel _viewModel;

        private void AddApplicationButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}