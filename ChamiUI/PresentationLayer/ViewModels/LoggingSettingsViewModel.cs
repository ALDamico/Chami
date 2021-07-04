namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// Viewmodel for the <see cref="LogginSettingsEditor"/> control.
    /// </summary>
    public class LoggingSettingsViewModel : SettingCategoryViewModelBase
    {
        private bool _loggingEnabled;

        /// <summary>
        /// Determines if the Chami application will log error messages or not.
        /// </summary>
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