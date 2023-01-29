using System;
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
    }
}