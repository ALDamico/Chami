using ChamiUI.PresentationLayer.Progress;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ChamiDbMigrations.Entities;

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

        public async Task ExecuteAsync(IProgress<CmdExecutorProgress> progress, float percentage, CancellationToken cancellationToken)
        {
            var arguments = $"/C REG delete HKCU\\Environment /F /V {EnvironmentVariable.Name}";
            var fullCmd = "cmd.exe " + arguments;
            progress?.Report(new CmdExecutorProgress(0, null, fullCmd));
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", arguments);
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.CreateNoWindow = true;
            Process process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();
            await process.WaitForExitAsync(cancellationToken);
            progress?.Report(new CmdExecutorProgress((int) percentage, process.StandardOutput.BaseStream, null));
        }
    }
}