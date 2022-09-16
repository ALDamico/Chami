using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Chami.CmdExecutor.Progress;

namespace Chami.CmdExecutor
{
    /// <summary>
    /// Base class for executing queues of Windows shell commands.
    /// </summary>
    public class CmdExecutorBase
    {
        /// <summary>
        /// Constructs a new <see cref="CmdExecutor"/> object and initializes its command queue.
        /// </summary>
        public CmdExecutorBase()
        {
            CommandQueue = new Queue<IShellCommand>();
            Progress = new Progress<CmdExecutorProgress>();
        }

        public void SetProgressHandler(Action<CmdExecutorProgress> handler)
        {
            Progress = new Progress<CmdExecutorProgress>(handler);
        }
        
        public static string StartingExecutionMessage { get; set; }
        public static string CompletedExecutionMessage { get; set; }
        public static string UnknownProcessAlreadyExited { get; set; }
        public static string KnownProcessAlreadyExited { get; set; }
        public static string KnownProcessTerminated { get; set; }

        /// <summary>
        /// The sequence of commands to execute.
        /// </summary>
        protected Queue<IShellCommand> CommandQueue { get; }

        /// <summary>
        /// Adds a new command to the queue.
        /// </summary>
        /// <param name="command">The command to add to the execution queue.</param>
        public void AddCommand(IShellCommand command)
        {
            if (command != null)
            {
                command.Progress = Progress;
            }
            CommandQueue.Enqueue(command);
        }

        /// <summary>
        /// Executes all commands in the <see cref="CommandQueue"/> synchronously and removes them from the data structure.
        /// </summary>
        public virtual void Execute()
        {
            do
            {
                var environmentVariable = CommandQueue.Dequeue();
                environmentVariable.Execute();
            } while (CommandQueue.Count > 0);
        }

        /// <summary>
        /// Executes all commands in the <see cref="CommandQueue"/> asynchronously and removes them from the data structure.
        /// Optionally, notifies of progress.
        /// Can be canceled.
        /// </summary>
        /// <param name="cancellationToken">Enables canceling</param>
        /// <seealso cref="IProgress{T}"/> 
        public virtual async Task ExecuteAsync(
            CancellationToken cancellationToken)
        {
            var message = StartingExecutionMessage;
            CmdExecutorProgress cmdExecutorProgress = new CmdExecutorProgress(0, null, message);
            Progress?.Report(cmdExecutorProgress);
            int currentIndex = 0;
            int count = CommandQueue.Count;
            do
            {
                var environmentVariable = CommandQueue.Dequeue();
                currentIndex++;
                float percentage = 100.0F * currentIndex / count;
                try
                {
                    await environmentVariable.ExecuteAsync(percentage, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    environmentVariable.TerminateProcess(percentage);
                    throw;
                }
            } while (CommandQueue.Count > 0);


            Progress?.Report(new CmdExecutorProgress(100, CompletedExecutionMessage));
        }
        
        public IProgress<CmdExecutorProgress> Progress { get; set; }

        public void ClearCommandQueue()
        {
            CommandQueue.Clear();
        }
    }
}