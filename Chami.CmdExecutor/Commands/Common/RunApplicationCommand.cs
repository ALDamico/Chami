using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Chami.CmdExecutor;
using Microsoft.VisualBasic;

namespace ChamiUI.BusinessLayer.Commands;

public class RunApplicationCommand : ShellCommandBase
{
    public override void Execute()
    {
        if (!File.Exists(Path))
        {
            throw new InvalidOperationException();
        }

        var process = PrepareProcess(null);
        process.Start();
        Process = process;
    }
    
    public Process Process { get; private set; }
    
    public string Path { get; set; }

    public override Task ExecuteAsync(float percentage, CancellationToken cancellationToken)
    {
        throw new System.NotSupportedException();
    }

    public RunApplicationCommand(string path)
    {
        Path = path;
    }

    protected override Process PrepareProcess(string arguments)
    {
        ProcessStartInfo processStartInfo = new ProcessStartInfo(Path, arguments)
        {
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            CreateNoWindow = false
        };
        Process process = new Process()
        {
            StartInfo = processStartInfo
        };
        return process;
    }
}