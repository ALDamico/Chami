using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ChamiUI.DataLayer.Entities;
using ChamiUI.PresentationLayer.Progress;

namespace ChamiUI.BusinessLayer
{
    public class EnvironmentVariableRemovalCommand : IEnvironmentVariableCommand
    {
        public EnvironmentVariableRemovalCommand(EnvironmentVariable variable)
        {
            EnvironmentVariable = variable;
        }

        public EnvironmentVariable EnvironmentVariable { get; set; }

        public void Execute()
        {
            var arguments = $"/C REG delete HKCU\\Environment /F /V {EnvironmentVariable.Name}";
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", arguments);
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardOutput = true;
            Process process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();
        }

        public async Task ExecuteAsync(IProgress<CmdExecutorProgress> progress = null)
        {
            var arguments = $"/C REG delete HKCU\\Environment /F /V {EnvironmentVariable.Name}";
            var fullCmd = "cmd.exe " + arguments;
            progress?.Report(new CmdExecutorProgress(0, null, fullCmd));
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", arguments);
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardOutput = true;
            Process process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();
            await process.WaitForExitAsync();
            progress?.Report(new CmdExecutorProgress(0, process.StandardOutput.BaseStream, null));
        }
    }
}