using System.Collections.ObjectModel;
using System.Windows.Media;
using ChamiUI.BusinessLayer.Logger;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;
using Serilog.Events;

namespace ChamiUI.BusinessLayer.Services;

public class LoggingService
{
    public LoggingService()
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
    public ObservableCollection<LogLevelViewModel> AvailableLogLevels { get; }
    
    public bool LoggingEnabled { get; set; }
    public ChamiLogger ChamiLogger { get; set; }

    public void SetMinimumLogLevel(LogEventLevel logEventLevel)
    {
        ChamiLogger.ChangeMinimumLevel(logEventLevel);
    }

    private LogEventLevel _minimumLogLevel;

    public LogEventLevel MinimumLogLevel
    {
        get => _minimumLogLevel;
        set
        {
            _minimumLogLevel = value;
            SetMinimumLogLevel(value);
        }
    }
}