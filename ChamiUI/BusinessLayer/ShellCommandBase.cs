using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Chami.Db.Entities;
using ChamiUI.PresentationLayer.Progress;

namespace ChamiUI.BusinessLayer
{
    public abstract class ShellCommandBase : IShellCommand
    {
        public abstract void Execute();

        public abstract Task ExecuteAsync(IProgress<CmdExecutorProgress> progress, float percentage,
            CancellationToken cancellationToken);

        /// <summary>
        /// Creates a new <see cref="Process"/> object that can be started by an inheritor.
        /// </summary>
        /// <param name="arguments">The command line to execute by the process.</param>
        /// <returns>A <see cref="Process"/> object for use by an <see cref="CmdExecutorBase"/>.</returns>
        protected virtual Process PrepareProcess(string arguments)
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
    }
}