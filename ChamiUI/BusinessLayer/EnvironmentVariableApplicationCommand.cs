using System.Threading;
using System.Threading.Tasks;
using Chami.CmdExecutor;
using Chami.CmdExecutor.Progress;
using Chami.Db.Entities;

namespace ChamiUI.BusinessLayer
{
    /// <summary>
    /// A command that creates (persistently) a new environment variable or updates its value if it already exists.
    /// </summary>
    public class EnvironmentVariableApplicationCommand : ShellCommandBase
    {
        public EnvironmentVariableApplicationCommand(EnvironmentVariable viewModel)
        {
            EnvironmentVariable = viewModel;
        }
        public EnvironmentVariable EnvironmentVariable { get; set; }
        /// <summary>
        /// Execute the shell command.
        /// </summary>
        public override void Execute()
        {
            var arguments = $"/C SETX {EnvironmentVariable.Name} {EnvironmentVariable.Value}";
            var process = PrepareProcess(arguments);
            process.Start();
        }

        /// <summary>
        /// Executes the shell command asynchronously.
        /// Optionally reports progress.
        /// Can be canceled.
        /// </summary>
        /// <param name="percentage">The progress percentage.</param>
        /// <param name="cancellationToken">Allows task cancellation.</param>
        public override async Task ExecuteAsync(float percentage,
            CancellationToken cancellationToken)
        {
            var arguments = $"/C SETX \"{EnvironmentVariable.Name}\" \"{EnvironmentVariable.Value}\"";
            var commandLineFull = "cmd.exe " + arguments;
            Progress?.Report(new CmdExecutorProgress((int) percentage, commandLineFull));
            var process = PrepareProcess(arguments);
            
            process.Start();
            SubscribeToAllOutput((_, args) =>
            {
                Progress?.Report(new CmdExecutorProgress((int)percentage, args.Data));
            });
            
            await process.WaitForExitAsync(cancellationToken);
        }
    }
}