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
    }
}