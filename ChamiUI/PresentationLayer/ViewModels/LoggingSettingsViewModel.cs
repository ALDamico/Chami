using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Windows.Media;
using ChamiUI.BusinessLayer.Annotations;
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
        public LoggingSettingsViewModel()
        {
            AvailableLogLevels = new ObservableCollection<LogLevelViewModel>();
            // Verbose
            AvailableLogLevels.Add(new LogLevelViewModel()
            {
                DisplayName = ChamiUIStrings.LogLevelVerbose,
                Description = ChamiUIStrings.LogLevelVerboseDescription,
                BackingValue = LogEventLevel.Verbose,
                ForegroundColor = Brushes.Black
            });
            
            // Debug
            AvailableLogLevels.Add(new LogLevelViewModel()
            {
                DisplayName = ChamiUIStrings.LogLevelDebug,
                Description = ChamiUIStrings.LogLevelDebugDescription,
                BackingValue = LogEventLevel.Debug,
                ForegroundColor = Brushes.DimGray
            });
            
            // Information
            AvailableLogLevels.Add(new LogLevelViewModel()
            {
                DisplayName = ChamiUIStrings.LogLevelInformation,
                Description = ChamiUIStrings.LogLevelInformationDescription,
                BackingValue = LogEventLevel.Information,
                ForegroundColor = Brushes.Green
            });
            
            //Warning
            AvailableLogLevels.Add(new LogLevelViewModel()
            {
                DisplayName = ChamiUIStrings.LogLevelWarning,
                Description = ChamiUIStrings.LogLevelWarningDescription,
                BackingValue = LogEventLevel.Warning,
                ForegroundColor = Brushes.Orange
            });
            
            // Error
            AvailableLogLevels.Add(new LogLevelViewModel()
            {
                DisplayName = ChamiUIStrings.LogLevelError,
                Description = ChamiUIStrings.LogLevelErrorDescription,
                BackingValue = LogEventLevel.Error,
                ForegroundColor = Brushes.Red
            });
            
            // Fatal
            AvailableLogLevels.Add(new LogLevelViewModel()
            {
                DisplayName = ChamiUIStrings.LogLevelFatal,
                Description = ChamiUIStrings.LogLevelFatalDescription,
                BackingValue = LogEventLevel.Fatal,
                ForegroundColor = Brushes.DarkRed
            });
        }
        
        private bool _loggingEnabled;
        private LogLevelViewModel _selectedMinimumLogLevel;
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

                SelectedMinimumLogLevel = AvailableLogLevels.FirstOrDefault(l => l.BackingValue == _minimumLogLevel);
                
                OnPropertyChanged(nameof(MinimumLogLevel));
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
                    _minimumLogLevel = value.BackingValue;
                }
                
                OnPropertyChanged(nameof(SelectedMinimumLogLevel));
                OnPropertyChanged(nameof(MinimumLogLevel));
            }
        }
        
        public ObservableCollection<LogLevelViewModel> AvailableLogLevels { get; }
    }
}