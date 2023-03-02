using System.Threading.Tasks;
using System.Windows;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;
using Serilog;

namespace ChamiUI.Windows.DetectedApplicationsWindow
{
    public partial class DetectedApplicationsWindow
    {
        public DetectedApplicationsWindow(DetectedApplicationsViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = _viewModel;
            InitializeComponent();
        }

        private readonly DetectedApplicationsViewModel _viewModel;

        public void OnApplicationsDetected(object sender, ApplicationsDetectedEventArgs args)
        {
            if (args == null) return;
            Log.Logger.Debug("The following applications are running: {@WatchedApplications}", args.DetectedApplications);
            var detectedApplications = args.DetectedApplications;
            foreach (var detectedApplication in detectedApplications)
            {
                _viewModel.DetectedApplications.Add(detectedApplication);
            }
        }
    }
}