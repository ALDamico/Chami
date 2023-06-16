using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Windows.Media;
using ChamiUI.BusinessLayer.Annotations;
using ChamiUI.BusinessLayer.Services;
using ChamiUI.Controls;
using ChamiUI.Localization;
using Serilog.Events;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// Viewmodel for the <see cref="LoggingSettingsEditor"/> control.
    /// </summary>
    public class LoggingSettingsViewModel : GenericLabelViewModel
    {
        private readonly LoggingService _loggingService;
        public LoggingSettingsViewModel(LoggingService loggingService)
        {
            _loggingService = loggingService;
        }
        
        private LogLevelViewModel _selectedMinimumLogLevel;

        /// <summary>
        /// Determines if the Chami application will log error messages or not.
        /// </summary>
        public bool LoggingEnabled
        {
            get => _loggingService.LoggingEnabled;
            set
            {
                _loggingService.LoggingEnabled = value;
                OnPropertyChanged();
            }
        }

        public LogEventLevel MinimumLogLevel
        {
            get => _loggingService.MinimumLogLevel;
            set
            {
                _loggingService.MinimumLogLevel = value;

                SelectedMinimumLogLevel = AvailableLogLevels.FirstOrDefault(l => l.BackingValue == _loggingService.MinimumLogLevel);
                
                OnPropertyChanged();
            }
        }

        public LogLevelViewModel SelectedMinimumLogLevel
        {
            get => _selectedMinimumLogLevel;
            set
            {
                _selectedMinimumLogLevel = value;

                if (value != null)
                {
                    _loggingService.MinimumLogLevel = value.BackingValue;
                }
                
                OnPropertyChanged();
                OnPropertyChanged(nameof(MinimumLogLevel));
            }
        }

        public ObservableCollection<LogLevelViewModel> AvailableLogLevels => _loggingService.AvailableLogLevels;
    }
}