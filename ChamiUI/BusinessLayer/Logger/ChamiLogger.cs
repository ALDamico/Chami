using Serilog;

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
        }

        /// <summary>
        /// Adds a file to write to.
        /// </summary>
        /// <param name="filename">The path to the log file.</param>
        public void AddFileSink(string filename)
        {
            _loggerConfiguration.WriteTo.File(filename);
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