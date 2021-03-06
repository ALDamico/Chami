using System.Threading.Tasks;
using System.Windows;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;
using Serilog;

namespace ChamiUI.Windows.DetectedApplicationsWindow
{
    public partial class DetectedApplicationsWindow
    {
        public DetectedApplicationsWindow()
        {
            _viewModel = new DetectedApplicationsViewModel();
            DataContext = _viewModel;
            InitializeComponent();
        }

        private DetectedApplicationsViewModel _viewModel;
        

        public void OnApplicationsDetected(object sender, ApplicationsDetectedEventArgs args)
        {
            if (args != null)
            {
                Log.Logger.Debug("The following applications are running: {@WatchedApplications}", args.DetectedApplications);
                var detectedApplications = args.DetectedApplications;
                foreach (var detectedApplication in detectedApplications)
                {
                    _viewModel.DetectedApplications.Add(detectedApplication);
                }
            }
        }

        private void RefreshButton_OnClick(object sender, RoutedEventArgs e)
        {
           _viewModel.RefreshDetection();
        }

        private async void TerminateMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            await _viewModel.TerminateSelectedApplication();
            _viewModel.RefreshDetection();
        }

        private async void TerminateAllMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            await TerminateAll();
        }

        private void CloseDetectedApplicationsWindowButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async Task TerminateAll()
        {
            await _viewModel.TerminateAll();
            _viewModel.RefreshDetection();
        }

        private async  void TerminateAllButton_OnClick(object sender, RoutedEventArgs e)
        {
            await TerminateAll();
        }
    }
}