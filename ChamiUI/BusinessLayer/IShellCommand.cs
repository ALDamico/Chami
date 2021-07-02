using ChamiUI.PresentationLayer.Progress;
using System;
using System.Threading;
using System.Threading.Tasks;
using Chami.Db.Entities;

namespace ChamiUI.BusinessLayer
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
        /// <param name="progress">Object to report progress to caller.</param>
        /// <param name="percentage">Execution percentage. Used by <see cref="CmdExecutorBase"/> for progress reporting.</param>
        /// <param name="cancellationToken">Allows cancelling the task.</param>
        /// <returns>Asynchronous method.</returns>
        /// <seealso cref="IProgress{T}"/>
        Task ExecuteAsync(IProgress<CmdExecutorProgress> progress, float percentage, CancellationToken cancellationToken);
    }
}