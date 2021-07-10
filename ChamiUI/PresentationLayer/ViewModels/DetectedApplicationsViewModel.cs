using System.Collections.ObjectModel;
using System.Windows;
using ChamiUI.BusinessLayer;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class DetectedApplicationsViewModel:ViewModelBase
    {
        public DetectedApplicationsViewModel()
        {
            DetectedApplications = new ObservableCollection<WatchedApplicationViewModel>();
            _detector = new RunningApplicationDetector(((App) Application.Current).Settings.WatchedApplicationSettings.WatchedApplications);
        }
        private RunningApplicationDetector _detector;
        public ObservableCollection<WatchedApplicationViewModel> DetectedApplications { get; }

        public void RefreshDetection()
        {
            DetectedApplications.Clear();
            var newApplications = _detector.Detect();
            if (newApplications == null)
            {
                return;
            }
            foreach (var app in newApplications)
            {
                DetectedApplications.Add(app);
            }
        }
    }
}