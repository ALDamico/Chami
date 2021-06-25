using System.Collections.ObjectModel;
using System.Linq;
using System;
using ChamiUI.Localization;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class WatchedApplicationControlViewModel : SettingCategoryViewModelBase
    {
        public WatchedApplicationControlViewModel()
        {
            WatchedApplications = new ObservableCollection<WatchedApplicationViewModel>();
        }
        private bool _isDetectionEnabled;
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

        public bool ControlsEnabled => IsDetectionEnabled;

        public ObservableCollection<WatchedApplicationViewModel> WatchedApplications { get; set; }

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

        private string _newApplicationName;
        public string NewApplicationName
        {
            get => _newApplicationName;
            set
            {
                _newApplicationName = value;
                OnPropertyChanged(nameof(NewApplicationName));
            }
        }
    }
}
