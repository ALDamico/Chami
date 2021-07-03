using System.Diagnostics;

namespace ChamiUI.PresentationLayer.Utils
{
    /// <summary>
    /// Helper functions that deal with <see cref="Process"/>es.
    /// </summary>
    /// <seealso cref="Process"/>
    public static class ProcessUtils
    {
        /// <summary>
        /// Opens a link in a new browser window.
        /// </summary>
        /// <param name="address">The URL to open.</param>
        public static void OpenLinkInBrowser(string address)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo(address)
                {
                    // Required, otherwise the app crashes with a Win32Exception
                    UseShellExecute = true
                }
            };
            
            process.Start();
        }
    }
}