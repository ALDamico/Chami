using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Chami.CmdExecutor.Progress;

namespace Chami.CmdExecutor
{
    /// <summary>
    /// Base class that provides common protected methods for classes that implement <see cref="IShellCommand"/>.
    /// </summary>
    public abstract class ShellCommandBase : IShellCommand
    {
        /// <summary>
        /// Derived classes must implement the <see cref="Execute"/> method.
        /// </summary>
        public abstract void Execute();


        /// <summary>
        /// Derived classes must implement the <see cref="ExecuteAsync"/> method.
        /// </summary>
        /// <param name="percentage">Percentage of overall completion</param>
        /// <param name="cancellationToken">Allows cancelling the task.</param>
        /// <returns></returns>
        public abstract Task ExecuteAsync(float percentage,
            CancellationToken cancellationToken);

        public IProgress<CmdExecutorProgress> Progress { get; set; }

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
            ProcessToExecute = process;
            return process;
        }

        public void SubscribeToOutputReceived(DataReceivedEventHandler callback)
        {
            ProcessToExecute.BeginOutputReadLine();
            ProcessToExecute.OutputDataReceived += callback;
        }

        public void SubscribeToErrorReceived(DataReceivedEventHandler callback)
        {
            ProcessToExecute.BeginErrorReadLine();
            ProcessToExecute.ErrorDataReceived += callback;
        }

        public void SubscribeToAllOutput(DataReceivedEventHandler callback)
        {
            SubscribeToErrorReceived(callback);
            SubscribeToOutputReceived(callback);
        }

        public Process ProcessToExecute { get; private set; }

        public void TerminateProcess(float percentage)
        {
            string message;
            if (ProcessToExecute == null)
            {
                message = CmdExecutorBase.UnknownProcessAlreadyExited;
                Progress?.Report(new CmdExecutorProgress(percentage, message));
                return;
            }

            if (ProcessToExecute.HasExited)
            {
                message = string.Format(CmdExecutorBase.KnownProcessAlreadyExited, ProcessToExecute.ProcessName,
                    ProcessToExecute.Id);
                Progress?.Report(new CmdExecutorProgress(percentage, message));
                return;
            }

            try
            {
                message = string.Format(CmdExecutorBase.KnownProcessTerminated, ProcessToExecute.ProcessName,
                    ProcessToExecute.Id);
                ProcessToExecute.Kill();
                
                Progress?.Report(new CmdExecutorProgress(percentage, message));
            }
            catch (InvalidOperationException)
            {
                // The process has already exited
                message = string.Format(CmdExecutorBase.KnownProcessAlreadyExited, ProcessToExecute.ProcessName,
                    0);
                Progress?.Report(new CmdExecutorProgress(percentage, message));
            }
        }
    }
}