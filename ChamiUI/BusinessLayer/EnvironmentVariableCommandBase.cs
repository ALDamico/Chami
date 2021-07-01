using System;
using System.Threading;
using System.Threading.Tasks;
using Chami.Db.Entities;
using ChamiUI.PresentationLayer.Progress;

namespace ChamiUI.BusinessLayer
{
    public abstract class EnvironmentVariableCommandBase : IEnvironmentVariableCommand
    {
        public EnvironmentVariableCommandBase(EnvironmentVariable environmentVariable)
        {
            EnvironmentVariable = environmentVariable;
        }
        public EnvironmentVariable EnvironmentVariable { get; set; }
        public abstract void Execute();

        public abstract Task ExecuteAsync(IProgress<CmdExecutorProgress> progress, float percentage,
            CancellationToken cancellationToken);
    }
}