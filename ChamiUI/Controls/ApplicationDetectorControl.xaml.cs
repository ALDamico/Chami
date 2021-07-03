using ChamiUI.PresentationLayer.ViewModels;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System;
using ChamiUI.Localization;

namespace ChamiUI.Controls
{
    /// <summary>
    ///  This is the control in the Settings window that enables the user to detect if some application is running so
    /// that they can restart them manually.
    /// </summary>
    public partial class ApplicationDetectorControl
    {
        /// <summary>
        /// Constructs a new <see cref="ApplicationDetectorControl"/>,
        /// </summary>
        /// <param name="viewModel">The viewModel containing window state.</param>
        public ApplicationDetectorControl(WatchedApplicationControlViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = _viewModel;
            InitializeComponent();
        }

        private readonly WatchedApplicationControlViewModel _viewModel;

        /// <summary>
        /// Reacts to the user clicking the <see cref="AddApplicationButton"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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