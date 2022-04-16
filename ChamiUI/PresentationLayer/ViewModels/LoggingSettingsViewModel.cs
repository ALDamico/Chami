using ChamiUI.Controls;
using Serilog.Events;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// Viewmodel for the <see cref="LoggingSettingsEditor"/> control.
    /// </summary>
    public class LoggingSettingsViewModel : SettingCategoryViewModelBase
    {
        private bool _loggingEnabled;
        private LogEventLevel _minimumLogLevel;

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

        public LogEventLevel MinimumLogLevel
        {
            get => _minimumLogLevel;
            set
            {
                _minimumLogLevel = value;
                OnPropertyChanged(nameof(MinimumLogLevel));
            }
        }
    }
}