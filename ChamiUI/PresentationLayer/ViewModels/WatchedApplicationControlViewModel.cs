using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ChamiUI.BusinessLayer;
using ChamiUI.BusinessLayer.Annotations;
using ChamiUI.BusinessLayer.Commands;
using ChamiUI.BusinessLayer.Processes;
using ChamiUI.Localization;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// Viewmodel for the watched applications control.
    /// </summary>
    public class WatchedApplicationControlViewModel : GenericLabelViewModel
    {
        /// <summary>
        /// Constructs a new <see cref="WatchedApplicationControlViewModel"/>.
        /// </summary>
        public WatchedApplicationControlViewModel(ProcessLauncherService processLauncherService)
        {
            WatchedApplications = new ObservableCollection<WatchedApplicationViewModel>();
            _processLauncherService = processLauncherService;
        }
        private bool _isDetectionEnabled;
        private ProcessLauncherService _processLauncherService;
        private WatchedApplicationViewModel _selectedApplication;

        public void RunApplication(string applicationPath)
        {
            _processLauncherService.RunApplication(applicationPath);
        }
        
        /// <summary>
        /// True if Chami is tracking the detection of running applications, otherwise false.
        /// </summary>
        public bool IsDetectionEnabled
        {
            get => _isDetectionEnabled;
            set
            {
                _isDetectionEnabled = value;
                OnPropertyChanged(nameof(IsDetectionEnabled));
                OnPropertyChanged(nameof(ControlsEnabled));
            }
        }

        /// <summary>
        /// Determines if the controls that allow editing the watched applications are enabled.
        /// </summary>
        public bool ControlsEnabled => IsDetectionEnabled;

        /// <summary>
        /// The list of applications to watch.
        /// </summary>
        [NonPersistentSetting]
        public ObservableCollection<WatchedApplicationViewModel> WatchedApplications { get; set; }

        /// <summary>
        /// Adds a new watched application to the datastore and to the application.
        /// </summary>
        /// <returns>True if a new application has been added (i.e., it doesn't already exist in the datastore),
        /// otherwise false.</returns>
        /// <exception cref="InvalidOperationException">An <see cref="InvalidOperationException"/> is thrown if the name
        /// is null, an empty string, or just whitespace.</exception>
        public bool AddWatchedApplication()
        {
            if (string.IsNullOrWhiteSpace(NewApplicationName))
            {
                throw new InvalidOperationException(ChamiUIStrings.AddWatchedApplicationNullApplicationNameErrorMessage);
            }
            var name = NewApplicationName;
            if (WatchedApplications.Any(wa => wa.Name == name))
            {
                return false;
            }
            var appVm = new WatchedApplicationViewModel();
            appVm.IsWatchEnabled = true;
            appVm.Name = name;
            WatchedApplications.Add(appVm);
            return true;
        }

        /// <summary>
        /// Marks a <see cref="WatchedApplicationViewModel"/> for deletion so that it will be removed from the datastore
        /// when the settings are saved.
        /// </summary>
        /// <param name="viewModel">The <see cref="WatchedApplicationViewModel"/> to mark for deletion.</param>
        internal void DeleteWatchedApplication(WatchedApplicationViewModel viewModel)
        {
            viewModel.Id = -1;
        }

        private string _newApplicationName;
        
        /// <summary>
        /// The name of the application to add.
        /// </summary>
        [NonPersistentSetting]
        public string NewApplicationName
        {
            get => _newApplicationName;
            set
            {
                _newApplicationName = value;
                OnPropertyChanged(nameof(NewApplicationName));
            }
        }

        public bool IsPathColumnToolTipVisible => !string.IsNullOrWhiteSpace(SelectedApplication?.Path);

        public WatchedApplicationViewModel SelectedApplication
        {
            get => _selectedApplication;
            set
            {
                _selectedApplication = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsPathColumnToolTipVisible));
            }
        }

        public void ProcessIconPath(string iconPath)
        {
            using Bitmap bitmap = new Bitmap(iconPath);
            SelectedApplication.IconWidth = bitmap.Width;
            SelectedApplication.IconHeight = bitmap.Height;
            using var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            SelectedApplication.Icon = ms.ToArray();
        }
    }
}
