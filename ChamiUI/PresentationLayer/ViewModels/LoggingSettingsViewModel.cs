namespace ChamiUI.PresentationLayer.ViewModels
{
    public class LoggingSettingsViewModel : SettingCategoryViewModelBase
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