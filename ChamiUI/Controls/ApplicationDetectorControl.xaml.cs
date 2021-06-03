using ChamiUI.PresentationLayer.ViewModels;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System;
using ChamiUI.Localization;

namespace ChamiUI.Controls
{
    public partial class ApplicationDetectorControl
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
                MessageBox.Show(ex.Message, ChamiUIStrings.GenericExceptionMessageBoxCaption, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}