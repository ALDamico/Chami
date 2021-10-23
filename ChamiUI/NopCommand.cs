using System;
using System.Threading;
using System.Threading.Tasks;
using Chami.CmdExecutor;
using Chami.CmdExecutor.Progress;
using Chami.Db.Entities;
using ChamiUI.Localization;

namespace ChamiUI
{
    public class NopCommand : ShellCommandBase
    {
        public NopCommand(EnvironmentVariable environmentVariable)
        {
            _environmentVariable = environmentVariable;
        }

        private readonly EnvironmentVariable _environmentVariable;

        public override void Execute()
        {
            return;
        }

        public override Task ExecuteAsync(IProgress<CmdExecutorProgress> progress, float percentage,
            CancellationToken cancellationToken)
        {
            var message = string.Format(ChamiUIStrings.NopCommandMessage, _environmentVariable?.Name);
            progress?.Report(new CmdExecutorProgress(percentage, message));
            return Task.CompletedTask;
        }
    }
}