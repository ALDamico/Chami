using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.BusinessLayer.Logger;
using ChamiUI.PresentationLayer.ViewModels;
using Serilog.Core;
using System;
using System.Configuration;
using System.Windows;
using System.Windows.Threading;

namespace ChamiUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
#if !DEBUG
            DispatcherUnhandledException += ShowExceptionMessageBox;
#endif
            Logger = new ChamiLogger();
            Logger.AddFileSink("chami.log");
        }

        static App()
        {
            _settings = new SettingsDataAdapter(GetConnectionString()).GetSettings();
        }

        public ChamiLogger Logger { get; }

        private static SettingsViewModel _settings;

        public static SettingsViewModel GetSettings()
        {
            return _settings;
        }

        public static string GetConnectionString()
        {
            var chamiDirectory = Environment.CurrentDirectory;
            return String.Format(ConfigurationManager.ConnectionStrings["Chami"].ConnectionString, chamiDirectory);
        }

        public void ShowExceptionMessageBox(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            var exceptionMessage = args.Exception.Message;
            args.Handled = true;
            MessageBox.Show(exceptionMessage, "An exception occurred!", MessageBoxButton.OK, MessageBoxImage.Error);
            if (_settings.LoggingSettings.LoggingEnabled)
            {
                var logger = Logger.GetLogger();
                logger.Error(exceptionMessage);
                logger.Error(args.Exception.StackTrace);
            }
        }

        public Logger GetLogger()
        {
            return Logger.GetLogger();
        }
    }
}