using Serilog;
using Serilog.Configuration;
using Serilog.Core;

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

        public Serilog.Core.Logger GetLogger()
        {
            return _loggerConfiguration.CreateLogger();
        }
    }
}