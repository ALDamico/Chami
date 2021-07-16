using Serilog;

namespace ChamiUI.BusinessLayer.Logger
{
    /// <summary>
    /// Provides logging functionality to the Chami application.
    /// </summary>
    public class ChamiLogger
    {
        private LoggerConfiguration _loggerConfiguration;

        /// <summary>
        /// Constructs a new <see cref="ChamiLogger"/> object and initializes its <see cref="LoggerConfiguration"/>
        /// </summary>
        public ChamiLogger()
        {
            _loggerConfiguration = new LoggerConfiguration();
        }

        /// <summary>
        /// Adds a file to write to.
        /// </summary>
        /// <param name="filename">The path to the log file.</param>
        public void AddFileSink(string filename)
        {
            _loggerConfiguration.WriteTo.File(filename);
        }

        private static Serilog.Core.Logger _logger;

        /// <summary>
        /// Gets the Serilog Logger object.
        /// </summary>
        /// <returns>A static instance of the Serilog logger.</returns>
        public Serilog.Core.Logger GetLogger()
        {
            if (_logger == null)
            {
                _logger = _loggerConfiguration.CreateLogger();
            }

            return _logger;
        }
    }
}