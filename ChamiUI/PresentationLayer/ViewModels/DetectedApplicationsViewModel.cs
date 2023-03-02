using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AsyncAwaitBestPractices.MVVM;
using ChamiUI.BusinessLayer;
using Serilog;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class DetectedApplicationsViewModel : ViewModelBase
    {
        public DetectedApplicationsViewModel()
        {
            DetectedApplications = new ObservableCollection<WatchedApplicationViewModel>();
            _detector = new RunningApplicationDetector(((App) Application.Current).Settings.WatchedApplicationSettings
                .WatchedApplications);
            KillApplicationsCommand =
                new AsyncCommand<IEnumerable<WatchedApplicationViewModel>>(ExecuteKillApplications);
            RefreshDetectionCommand = new AsyncCommand(ExecuteDetection);
        }

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
                await KillProcessByPid(SelectedApplication.Pid);
            }
            else
            {
                var tasks = watchedApplicationViewModels
                    .Select(watchedApplication => KillProcessByPid(watchedApplication.Pid)).ToList();

                await Task.WhenAll(tasks);
            }

            RefreshDetection();
        }

        private readonly RunningApplicationDetector _detector;
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

        private async Task KillProcessByPid(int pid)
        {
            if (pid == 4)
            {
                return;
            }

            var process = Process.GetProcessById(pid);
            try
            {
                process.Kill();
                await process.WaitForExitAsync();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "{ex}");
            }
        }

        public IAsyncCommand<IEnumerable<WatchedApplicationViewModel>> KillApplicationsCommand { get; }
        public IAsyncCommand RefreshDetectionCommand { get; }
    }
}