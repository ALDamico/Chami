using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using ChamiUI.Windows.Abstract;

namespace ChamiUI.Utils
{
    public static class AppUtils
    {
        public static string GetLogFilePath()
        {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/chami.log";
        }

        public static string GetApplicationExecutablePath()
        {
            return Assembly.GetEntryAssembly().Location.Replace(".dll", ".exe");
        }

        public static string GetApplicationFolder()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public static App GetChamiApp()
        {
            return Application.Current as App;
        }

        public static IServiceProvider GetAppServiceProvider()
        {
            return GetChamiApp().ServiceProvider;
        }

        public static string GetConnectionString()
        {
            var chamiDirectory = AppUtils.GetApplicationFolder();
            try
            {
                return String.Format(ConfigurationManager.ConnectionStrings["Chami"].ConnectionString, chamiDirectory);
            }
            catch (NullReferenceException)
            {
                // A unit test is running. Use its connection string instead
                return "Data Source=|DataDirectory|InputFiles/chami.db;Version=3;";
            }
        }

        public static ChamiWindow GetMainWindow()
        {
            return (ChamiWindow) Application.Current.MainWindow;
        }
        public static string GetRuntimeInfo()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            var applicationName = NotAvailableString;
            var applicationVersion = NotAvailableString;
            var clrVersion = Environment.Version.ToString();
            var commandLine = Environment.CommandLine;
            var processId = Environment.ProcessId.ToString();
            var arguments = NoArguments;
            // The first element in the command line arguments array is the application itself
            if (Environment.GetCommandLineArgs().Length > 1)
            {
                arguments = string.Join(", ", Environment.GetCommandLineArgs().Skip(1));
            }

            if (entryAssembly != null) 
            {
                applicationName = entryAssembly.GetName().Name;
                var assemblyVersion = entryAssembly.GetName().Version;
                if (assemblyVersion != null)
                {
                    applicationVersion = assemblyVersion.ToString();    
                }
                
            }

            return $@"Application name: {applicationName}
Application version: {applicationVersion}
CLR information:
CLR version: {clrVersion}
Command line: {commandLine}
Arguments: {arguments}
Process ID: {processId}
";
        }
        
        public const string NotAvailableString = "N/A";
        public const string NoArguments = "None";
    }
}