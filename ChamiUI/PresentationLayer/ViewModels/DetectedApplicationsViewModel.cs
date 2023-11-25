using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AsyncAwaitBestPractices.MVVM;
using ChamiUI.BusinessLayer.Services;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class DetectedApplicationsViewModel : ViewModelBase
    {
        public DetectedApplicationsViewModel(WatchedApplicationService watchedApplicationService)
        {
            DetectedApplications = new ObservableCollection<WatchedApplicationViewModel>();
            KillApplicationsCommand =
                new AsyncCommand<IEnumerable<WatchedApplicationViewModel>>(ExecuteKillApplications);
            RefreshDetectionCommand = new AsyncCommand(ExecuteDetection);
            _watchedApplicationService = watchedApplicationService;
        }

        private readonly WatchedApplicationService _watchedApplicationService;

        private async Task ExecuteDetection()
        {
            RefreshDetection();
            await Task.CompletedTask;
        }

        private async Task ExecuteKillApplications(
            IEnumerable<WatchedApplicationViewModel> watchedApplicationViewModels)
        {
            if (watchedApplicationViewModels == null)
            {
                await _watchedApplicationService.KillProcessByPid(SelectedApplication.Pid);
            }
            else
            {
                var tasks = watchedApplicationViewModels
                    .Select(watchedApplication => _watchedApplicationService.KillProcessByPid(watchedApplication.Pid)).ToList();

                await Task.WhenAll(tasks);
            }

            RefreshDetection();
        }
        public ObservableCollection<WatchedApplicationViewModel> DetectedApplications { get; }

        private WatchedApplicationViewModel _selectedApplication;

        public WatchedApplicationViewModel SelectedApplication
        {
            get => _selectedApplication;
            set
            {
                _selectedApplication = value;
                OnPropertyChanged();
            }
        }

        private void RefreshDetection()
        {
            DetectedApplications.Clear();
            var newApplications = _watchedApplicationService.Detect();
            if (newApplications == null)
            {
                return;
            }

            foreach (var app in newApplications)
            {
                DetectedApplications.Add(app);
            }
        }

        public IAsyncCommand<IEnumerable<WatchedApplicationViewModel>> KillApplicationsCommand { get; }
        public IAsyncCommand RefreshDetectionCommand { get; }
    }
}