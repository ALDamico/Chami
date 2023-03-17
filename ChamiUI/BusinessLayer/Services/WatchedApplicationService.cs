using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.PresentationLayer.ViewModels;
using Serilog;

namespace ChamiUI.BusinessLayer.Services;

public class WatchedApplicationService
{
    public WatchedApplicationService(WatchedApplicationDataAdapter watchedApplicationDataAdapter, RunningApplicationDetector runningApplicationDetector)
    {
        _watchedApplicationDataAdapter = watchedApplicationDataAdapter;
        _runningApplicationDetector = runningApplicationDetector;
    }
    private readonly WatchedApplicationDataAdapter _watchedApplicationDataAdapter;
    private RunningApplicationDetector _runningApplicationDetector;

    public List<WatchedApplicationViewModel> Detect()
    {
        return _runningApplicationDetector.Detect();
    }
    
    public async Task KillProcessByPid(int pid)
    {
        if (pid == 4)
        {
            return;
        }

        var process = Process.GetProcessById(pid);
        try
        {
            process.Kill();
            await process.WaitForExitAsync();
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "{ex}");
        }
    }
}