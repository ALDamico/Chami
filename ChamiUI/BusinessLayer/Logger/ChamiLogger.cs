using Serilog;
using Serilog.Configuration;

namespace ChamiUI.BusinessLayer.Logger
{
    public class ChamiLogger
    {
        private LoggerConfiguration _loggerConfiguration;

        public ChamiLogger()
        {
            _loggerConfiguration = new LoggerConfiguration();

        }

        public void AddFileSink(string filename)
        {
            var type = typeof(LoggerSinkConfiguration);
            _loggerConfiguration.WriteTo.File(filename);
        }

        private static Serilog.Core.Logger _logger;

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