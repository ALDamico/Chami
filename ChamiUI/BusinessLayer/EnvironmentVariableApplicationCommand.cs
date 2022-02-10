using System.Text.RegularExpressions;
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
            _environmentVariable = viewModel;
        }

        private readonly EnvironmentVariable _environmentVariable;
        /// <summary>
        /// Execute the shell command.
        /// </summary>
        public override void Execute()
        {
            var arguments = GetCommandLineToExecute();
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
            var arguments = GetCommandLineToExecute();
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

        /// <summary>
        /// Constructs the command-line parameters used by this command.
        /// </summary>
        /// <returns>The command-line parameters used by this command.</returns>
        private string GetCommandLineToExecute()
        {
            var environmentVariableValue = _environmentVariable.Value;

            var regex = new Regex( @"(\\+)$", RegexOptions.Compiled); 
            environmentVariableValue = regex.Replace(environmentVariableValue, CustomMatchEvaluator);

            return $"/C SETX \"{_environmentVariable.Name}\" \"{environmentVariableValue}\"";
        }

        private string CustomMatchEvaluator(Match m)
        {
            if (m.Groups.Count > 1)
            {
                return m.Groups[1].Value + m.Groups[1].Value;
            }

            return string.Empty;
        }
    }
}