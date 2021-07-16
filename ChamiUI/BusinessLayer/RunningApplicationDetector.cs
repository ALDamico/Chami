using System;
using ChamiUI.PresentationLayer.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Gapotchenko.FX.Diagnostics;

namespace ChamiUI.BusinessLayer
{
    /// <summary>
    /// Detects if one or more application are running.
    /// </summary>
    public class RunningApplicationDetector
    {
        /// <summary>
        /// Constructs a new <see cref="RunningApplicationDetector"/> object and initializes its list of watched applications.
        /// </summary>
        /// <param name="watchedApplications">A list of <see cref="WatchedApplicationViewModel"/> objects.</param>
        public RunningApplicationDetector(IEnumerable<WatchedApplicationViewModel> watchedApplications)
        {
            WatchedApplications = new List<WatchedApplicationViewModel>(watchedApplications);
        }
        
        /// <summary>
        /// The list of <see cref="WatchedApplicationViewModel"/>s to observe.
        /// </summary>
        public List<WatchedApplicationViewModel> WatchedApplications { get;  set; }

        /// <summary>
        /// Start detecting running applications.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of <see cref="WatchedApplicationViewModel"/> objects.</returns>
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
                        var watchedApplicationOutput = new WatchedApplicationViewModel();
                        try
                        {
                            var processEnvironmentVariables = process.ReadEnvironmentVariables();
                            if (processEnvironmentVariables.ContainsKey("_CHAMI_ENV"))
                            {
                                watchedApplicationOutput.ChamiEnvironmentName =
                                    processEnvironmentVariables["_CHAMI_ENV"];
                            }

                            watchedApplicationOutput.ProcessName = process.ProcessName;
                            watchedApplicationOutput.Pid = process.Id;
                            watchedApplicationOutput.Name = application.Name;
                            
                            output.Add(watchedApplicationOutput);
                        }
                        catch (InvalidOperationException)
                        {
                            //The application has already been terminated
                        }
                        
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
