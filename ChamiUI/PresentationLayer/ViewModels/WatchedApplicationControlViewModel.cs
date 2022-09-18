using System.Collections.ObjectModel;
using System.Linq;
using System;
using ChamiUI.BusinessLayer;
using ChamiUI.BusinessLayer.Annotations;
using ChamiUI.BusinessLayer.Commands;
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
        public WatchedApplicationControlViewModel()
        {
            WatchedApplications = new ObservableCollection<WatchedApplicationViewModel>();
            _cmdExecutor = new CmdExecutor();
        }
        private bool _isDetectionEnabled;
        private readonly CmdExecutor _cmdExecutor;
        private WatchedApplicationViewModel _selectedApplication;

        public void RunApplication(string applicationPath)
        {
            _cmdExecutor.ClearCommandQueue();
            _cmdExecutor.AddCommand(new RunApplicationCommand(applicationPath));
            _cmdExecutor.Execute();
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
    }
}
