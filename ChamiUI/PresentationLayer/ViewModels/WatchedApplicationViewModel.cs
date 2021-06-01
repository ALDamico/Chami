namespace ChamiUI.PresentationLayer.ViewModels
{
    public class WatchedApplicationViewModel: ViewModelBase
    {
        public int Id { get; set; }
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

        private string _name;
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

        public string ProcessName
        {
            get => _processName;
            set
            {
                _processName = value;
                OnPropertyChanged(nameof(value));
            }
        }
    }
}
