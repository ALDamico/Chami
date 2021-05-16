using ChamiUI.DataLayer.Entities;
using ChamiUI.PresentationLayer.Progress;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ChamiUI.BusinessLayer
{
    public class EnvironmentVariableApplicationCommand : IEnvironmentVariableCommand
    {
        public EnvironmentVariableApplicationCommand(EnvironmentVariable variable)
        {
            EnvironmentVariable = variable;
        }

        public EnvironmentVariable EnvironmentVariable { get; set; }

        public void Execute()
        {
            var arguments = $"/C SETX {EnvironmentVariable.Name} {EnvironmentVariable.Value}";
            var process = PrepareProcess(arguments);
            process.Start();
        }

        private Process PrepareProcess(string arguments)
        {   
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", arguments)
            {
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            Process process = new Process()
            {
                StartInfo = processStartInfo
            };
            return process;
        }

        public async Task ExecuteAsync(IProgress<CmdExecutorProgress> progress)
        {
            var arguments = $"/C SETX \"{EnvironmentVariable.Name}\" \"{EnvironmentVariable.Value}\"";
            var commandLineFull = "cmd.exe " + arguments;
            var process = PrepareProcess(arguments);
            process.Start();
            await process.WaitForExitAsync();
            if (progress != null)
            {
                progress.Report(new CmdExecutorProgress(0, process.StandardOutput.BaseStream, commandLineFull));
            }
        }
    }
}