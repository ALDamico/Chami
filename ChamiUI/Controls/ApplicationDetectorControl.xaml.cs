using ChamiUI.PresentationLayer.ViewModels;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System;

namespace ChamiUI.Controls
{
    public partial class ApplicationDetectorControl : UserControl
    {
        public ApplicationDetectorControl(WatchedApplicationControlViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = _viewModel;
            InitializeComponent();
        }

        private readonly WatchedApplicationControlViewModel _viewModel;

        private void AddApplicationButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var added = _viewModel.AddWatchedApplication();
                if (added == false)
                {
                    // Play a system sound if we're trying to add an application that already exists
                    SystemSounds.Beep.Play();
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
    }
}