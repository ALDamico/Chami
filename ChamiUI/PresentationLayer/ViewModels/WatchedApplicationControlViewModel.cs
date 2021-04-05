using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class WatchedApplicationControlViewModel: ViewModelBase
    {
        public WatchedApplicationControlViewModel()
        {
            WatchedApplications = new ObservableCollection<WatchedApplicationViewModel>();
        }
        private bool _isWatchEnabled;
        public bool IsWatchEnabled
        {
            get => _isWatchEnabled;
            set
            {
                _isWatchEnabled = value;
                OnPropertyChanged(nameof(IsWatchEnabled));
            }
        }

        public ObservableCollection<WatchedApplicationViewModel> WatchedApplications { get; set; }
    }
}
