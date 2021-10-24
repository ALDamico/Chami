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

        public NopCommand(string message)
        {
            _message = message;
        }

        private string _message;

        private readonly EnvironmentVariable _environmentVariable;

        public override void Execute()
        {
            return;
        }

        public override Task ExecuteAsync(float percentage,
            CancellationToken cancellationToken)
        {
            if (_environmentVariable != null)
            {
                _message = string.Format(ChamiUIStrings.NopCommandMessage, _environmentVariable?.Name);    
            }
            
            Progress?.Report(new CmdExecutorProgress(percentage, _message));
            return Task.CompletedTask;
        }
    }
}