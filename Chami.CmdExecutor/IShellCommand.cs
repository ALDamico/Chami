using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Chami.CmdExecutor.Progress;

namespace Chami.CmdExecutor
{
    /// <summary>
    /// Interface for executing shell commands for use by <see cref="CmdExecutorBase"/>.
    /// </summary>
    public interface IShellCommand
    {
        /// <summary>
        /// Execute a shell command synchronously.
        /// </summary>
        void Execute();

        /// <summary>
        /// Execute a shell command asynchronously.
        /// Can optionally report progress to caller.
        /// Can be cancelled.
        /// </summary>
        /// <param name="percentage">Execution percentage. Used by <see cref="CmdExecutorBase"/> for progress reporting.</param>
        /// <param name="cancellationToken">Allows cancelling the task.</param>
        /// <returns>Asynchronous method.</returns>
        /// <seealso cref="IProgress{T}"/>
        Task ExecuteAsync(float percentage, CancellationToken cancellationToken);
        
        IProgress<CmdExecutorProgress> Progress { get; set; }
        Process ProcessToExecute {get;}
        void TerminateProcess(float percentage);
    }
}