using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows;

namespace ChamiUI.Utils
{
    public static class AppUtils
    {
        public static string GetLogFilePath()
        {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/chami.log" ;
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
    }
}