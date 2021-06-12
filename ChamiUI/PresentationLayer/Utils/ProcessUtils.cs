using System.Diagnostics;

namespace ChamiUI.PresentationLayer.Utils
{
    public static class ProcessUtils
    {
        public static void OpenLinkInBrowser(string address)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(address);
            // Required, otherwise the app crashes with a Win32Exception
            startInfo.UseShellExecute = true;
            Process process = new Process();
            process.StartInfo = startInfo;

            process.Start();
        }
    }
}