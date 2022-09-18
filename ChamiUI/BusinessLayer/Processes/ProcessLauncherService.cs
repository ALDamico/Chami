using System.Collections.Generic;
using System.Diagnostics;
using ChamiUI.BusinessLayer.Commands;

namespace ChamiUI.BusinessLayer.Processes;

public class ProcessLauncherService
{
    public ProcessLauncherService()
    {
        _startedProcesses = new Dictionary<int, Process>();
        _cmdExecutor = new CmdExecutor();
    }
    
    public void RunApplication(string applicationPath)
    {
        _cmdExecutor.ClearCommandQueue();
        var command = new RunApplicationCommand(applicationPath);
        _cmdExecutor.AddCommand(command);
        _cmdExecutor.Execute();

        var process = command.Process;
        _startedProcesses[process.Id] = process;
    }

    public Process GetProcessById(int id)
    {
        _startedProcesses.TryGetValue(id, out var process);
        return process;
    }

    private readonly CmdExecutor _cmdExecutor;
    private readonly Dictionary<int, Process> _startedProcesses;
}