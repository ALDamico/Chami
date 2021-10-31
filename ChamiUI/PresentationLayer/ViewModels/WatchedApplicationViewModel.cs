using Chami.Plugins.Contracts.ViewModels;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// Viewmodel for watched applications.
    /// </summary>
    public class WatchedApplicationViewModel : ViewModelBase
    {
        /// <summary>
        /// The ID used by the datastore.
        /// </summary>
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
                OnPropertyChanged(nameof(MarkedForDeletion));
            }
        }

        private int _id;
        private bool _isWatchEnabled;

        /// <summary>
        /// True if Chami is detecting this application is running.
        /// </summary>
        public bool IsWatchEnabled
        {
            get => _isWatchEnabled;
            set
            {
                _isWatchEnabled = value;
                OnPropertyChanged(nameof(IsWatchEnabled));
            }
        }

        /// <summary>
        /// Determines if a viewmodel is marked for deletion.
        /// </summary>
        public bool MarkedForDeletion => Id < 0;

        private string _name;

        /// <summary>
        /// The name of the application.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _processName;

        /// <summary>
        /// The name of the process.
        /// </summary>
        public string ProcessName
        {
            get => _processName;
            set
            {
                _processName = value;
                OnPropertyChanged(nameof(value));
            }
        }

        private int _pid;

        public int Pid
        {
            get => _pid;
            set
            {
                _pid = value;
                OnPropertyChanged(nameof(Pid));
            }
        }

        private string _chamiEnvironmentName;

        public string ChamiEnvironmentName
        {
            get => _chamiEnvironmentName;
            set
            {
                _chamiEnvironmentName = value;
                OnPropertyChanged(nameof(ChamiEnvironmentName));
            }
        }
    }
}