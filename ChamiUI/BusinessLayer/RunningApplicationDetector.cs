using ChamiUI.PresentationLayer.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace ChamiUI.BusinessLayer
{
    public class RunningApplicationDetector
    {
        public RunningApplicationDetector(IEnumerable<WatchedApplicationViewModel> watchedApplications)
        {
            WatchedApplications = new List<WatchedApplicationViewModel>(watchedApplications);
        }
        public List<WatchedApplicationViewModel> WatchedApplications { get;  set; }

        public List<WatchedApplicationViewModel> Detect()
        {
            var processes = Process.GetProcesses();
            var output = new List<WatchedApplicationViewModel>();
            foreach (var process in processes)
            {
                foreach (var application in WatchedApplications.Where(a => a.IsWatchEnabled == true))
                {
                    var match = Regex.Match(process.ProcessName, application.Name, RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        application.ProcessName = process.ProcessName;
                        output.Add(application);
                    }
                }
            }
            if (output.Count == 0)
            {
                return null;
            }
            return output;
        }
    }
}
