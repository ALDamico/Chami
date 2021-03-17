using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using ChamiUI.DataLayer.Entities;
using ChamiUI.PresentationLayer.Progress;

namespace ChamiUI.BusinessLayer
{
    public class EnvironmentVariableApplicationCommand:IEnvironmentVariableCommand
    {
        public EnvironmentVariableApplicationCommand(EnvironmentVariable variable)
        {
            EnvironmentVariable = variable;
        }

        public EnvironmentVariable EnvironmentVariable { get; set; }

        public void Execute()
        {
            var arguments = $"/C SETX {EnvironmentVariable.Name} {EnvironmentVariable.Value}";
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", arguments);
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardOutput = true;
            Process process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();
        }

        public async Task ExecuteAsync(IProgress<CmdExecutorProgress> progress)
        {
            
            var arguments = $"/C SETX {EnvironmentVariable.Name} {EnvironmentVariable.Value}";
            var commandLineFull = "cmd.exe " + arguments;
            progress?.Report(new CmdExecutorProgress(0, null, commandLineFull));
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", arguments);
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardOutput = true;
            Process process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();
            await process.WaitForExitAsync();
            if (progress != null)
            {
                progress.Report(new CmdExecutorProgress(0, process.StandardOutput.BaseStream, null));
            }
            
        }
    }
}