namespace ChamiUI.PresentationLayer.ViewModels
{
    public class LoggingSettingsViewModel : ViewModelBase
    {
        private bool _loggingEnabled;

        public bool LoggingEnabled
        {
            get => _loggingEnabled;
            set
            {
                _loggingEnabled = value;
                OnPropertyChanged(nameof(LoggingEnabled));
            }
        }
    }
}