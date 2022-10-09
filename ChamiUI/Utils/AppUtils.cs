using System.IO;
using System.Reflection;

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
    }
}