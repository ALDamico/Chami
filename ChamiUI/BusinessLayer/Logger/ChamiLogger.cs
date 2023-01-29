using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace ChamiUI.BusinessLayer.Logger
{
    /// <summary>
    /// Provides logging functionality to the Chami application.
    /// </summary>
    public class ChamiLogger
    {
        private readonly LoggerConfiguration _loggerConfiguration;

        /// <summary>
        /// Constructs a new <see cref="ChamiLogger"/> object and initializes its <see cref="LoggerConfiguration"/>
        /// </summary>
        public ChamiLogger()
        {
            _loggerConfiguration = new LoggerConfiguration();
            _loggerConfiguration.Destructure.ByTransforming<EnvironmentChangedEventArgs>(args =>
                new
                {
                    Name = args?.NewActiveEnvironment?.Name,
                    NumberOfVariables = args?.NewActiveEnvironment?.EnvironmentVariables.Count
                });
            _loggerConfiguration.Destructure.ByTransforming<WatchedApplicationViewModel>(watchedApp =>
                new
                {
                    ProcessName = watchedApp.ProcessName,
                    ChamiEnvironmentName = watchedApp.ChamiEnvironmentName
                }
            );

            _loggerConfiguration.Destructure.ByTransforming<EnvironmentExistingEventArgs>(args => new {args.Name});
        }

        /// <summary>
        /// Adds a file to write to.
        /// </summary>
        /// <param name="filename">The path to the log file.</param>
        public void AddFileSink(string filename)
        {
            _loggerConfiguration.WriteTo.File(filename);
        }

        public void AddDebugSink()
        {
            _loggerConfiguration.WriteTo.Debug();
        }

        public void SetMinumumLevel(LogEventLevel minimumLevel)
        {
            _loggerConfiguration.MinimumLevel.ControlledBy(new LoggingLevelSwitch(minimumLevel));
        }

        /// <summary>
        /// Gets the Serilog Logger object.
        /// </summary>
        /// <returns>A static instance of the Serilog logger.</returns>
        public Serilog.Core.Logger GetLogger()
        {
            return _loggerConfiguration.CreateLogger();
        }
    }
}