using ChamiUI.PresentationLayer.Progress;
using System;
using System.Threading;
using System.Threading.Tasks;
using Chami.Db.Entities;

namespace ChamiUI.BusinessLayer
{
    /// <summary>
    /// Executes a shell command that removes an environment variable from the registry.
    /// </summary>
    public class EnvironmentVariableRemovalCommand : ShellCommandBase
    {
        /// <summary>
        /// Constructs a new <see cref="EnvironmentVariableRemovalCommand"/> object and sets the variable it applies to.
        /// </summary>
        /// <param name="variable"></param>
        public EnvironmentVariableRemovalCommand(EnvironmentVariable variable)
        {
            EnvironmentVariable = variable;
        }
        
        /// <summary>
        /// The environment variable to apply the command to.
        /// </summary>
        public EnvironmentVariable EnvironmentVariable { get; set; }

        /// <summary>
        /// Executes the shell command synchronously by deleting the registry key corresponding to the variable we want to remove.
        /// </summary>
        public override void Execute()
        {
            var arguments = $"/C REG delete HKCU\\Environment /F /V {EnvironmentVariable.Name}";
            var process = PrepareProcess(arguments);
            process.Start();
        }

        /// <summary>
        /// Executes the shell command asynchronously by deleting the registry key corresponding to the variable we want to remove.
        /// Optionally reports progress.
        /// Can be canceled.
        /// </summary>
        /// <param name="progress">Notifies the caller of progress.</param>
        /// <param name="percentage">Used for progress notification</param>
        /// <param name="cancellationToken">Enables cancelling the task.</param>
        public override async Task ExecuteAsync(IProgress<CmdExecutorProgress> progress, float percentage, CancellationToken cancellationToken)
        {
            var arguments = $"/C REG delete HKCU\\Environment /F /V {EnvironmentVariable.Name}";
            var fullCmd = "cmd.exe " + arguments;
            progress?.Report(new CmdExecutorProgress(0, null, fullCmd));
            var process = PrepareProcess(arguments);
            process.Start();
            await process.WaitForExitAsync(cancellationToken);
            progress?.Report(new CmdExecutorProgress((int) percentage, process.StandardOutput.BaseStream, null));
        }
    }
}