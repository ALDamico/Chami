using System;
using ChamiUI.PresentationLayer.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using ChamiUI.BusinessLayer.Processes;
using ChamiUI.PresentationLayer.Enums;
using Gapotchenko.FX.Diagnostics;
using Serilog;

namespace ChamiUI.BusinessLayer
{
    /// <summary>
    /// Detects if one or more application are running.
    /// </summary>
    public class RunningApplicationDetector
    {
        private readonly ProcessLauncherService _processLauncherService;

        /// <summary>
        /// Constructs a new <see cref="RunningApplicationDetector"/> object and initializes its list of watched applications.
        /// </summary>
        /// <param name="watchedApplications">A list of <see cref="WatchedApplicationViewModel"/> objects.</param>
        public RunningApplicationDetector(IEnumerable<WatchedApplicationViewModel> watchedApplications,
            ProcessLauncherService processLauncherService)
        {
            WatchedApplications = new List<WatchedApplicationViewModel>(watchedApplications);
            _processLauncherService = processLauncherService;
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
                foreach (var applicationName in WatchedApplications.Where(a => a.IsWatchEnabled).Select(a => a.Name))
                {
                    var match = Regex.Match(process.ProcessName, applicationName, RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        var watchedApplicationOutput = new WatchedApplicationViewModel();
                        var actualProcess = process;
                        try
                        {
                            var processId = process.Id;
                            var ownedProcess = _processLauncherService.GetProcessById(processId);
                            if (ownedProcess != null)
                            {
                                actualProcess = ownedProcess;
                            }
                            var processEnvironmentVariables = process.ReadEnvironmentVariables();
                            if (processEnvironmentVariables.ContainsKey("_CHAMI_ENV"))
                            {
                                watchedApplicationOutput.ChamiEnvironmentName =
                                    processEnvironmentVariables["_CHAMI_ENV"];
                            }

                            watchedApplicationOutput.ProcessName = process.ProcessName;
                            watchedApplicationOutput.Pid = processId;
                            watchedApplicationOutput.Name = applicationName;

                            output.Add(watchedApplicationOutput);
                        }
                        catch (InvalidOperationException)
                        {
                            //The application has already been terminated
                        }
                        DetectEnvironmentVariables(watchedApplicationOutput, actualProcess);
                    }
                }
            }
            return output;
        }

        private void DetectEnvironmentVariables(WatchedApplicationViewModel watchedApplicationViewModel, Process process)
        {
            try
            {
                var startInfo = process.StartInfo;
                var environmentVariables = startInfo.Environment;
                var environmentViewModel = watchedApplicationViewModel.Environment;

                foreach (var environmentVariable in environmentVariables)
                {
                    var variableName = environmentVariable.Key;
                    var variableValue = environmentVariable.Value;

                    if (variableName == "_CHAMI_ENV")
                    {
                        environmentViewModel.Name = variableValue;
                        continue;
                    }

                    var environmentVariableViewModel = new EnvironmentVariableViewModel();
                    environmentVariableViewModel.Environment = environmentViewModel;
                    environmentVariableViewModel.Name = variableName;
                    environmentVariableViewModel.Value = variableValue;
                    environmentViewModel.EnvironmentVariables.Add(environmentVariableViewModel);
                }
            }
            catch (Win32Exception ex)
            {
                Log.Logger.Error(ex, "{Ex}", ex);
                watchedApplicationViewModel.VariableDetectionStatus = VariableDetectionStatus.Failed;
                return;
            }
            catch (InvalidOperationException ex)
            {
                Log.Logger.Error(ex, "{Ex}", ex);
                watchedApplicationViewModel.VariableDetectionStatus = VariableDetectionStatus.Failed;
                return;
            }
            
            watchedApplicationViewModel.VariableDetectionStatus = VariableDetectionStatus.Successful;
        }
    }
}
