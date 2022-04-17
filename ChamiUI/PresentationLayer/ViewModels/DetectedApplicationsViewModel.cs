using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using ChamiUI.BusinessLayer;
using Serilog;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class DetectedApplicationsViewModel:ViewModelBase
    {
        public DetectedApplicationsViewModel()
        {
            DetectedApplications = new ObservableCollection<WatchedApplicationViewModel>();
            _detector = new RunningApplicationDetector(((App) Application.Current).Settings.WatchedApplicationSettings.WatchedApplications);
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
                OnPropertyChanged(nameof(SelectedApplication));
            }
        }

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

        public async Task TerminateSelectedApplication()
        {
            var pid = SelectedApplication.Pid;

            await KillProcessByPid(pid);
            
            
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

        public async Task TerminateAll()
        {
            var tasks = new List<Task>();
            foreach (var application in DetectedApplications)
            {
                tasks.Add(KillProcessByPid(application.Pid));
            }

            await Task.WhenAll(tasks);
        }
    }
}