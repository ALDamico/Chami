using ChamiUI.DataLayer.Entities;
using ChamiUI.PresentationLayer.Progress;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChamiUI.BusinessLayer
{
    public interface IEnvironmentVariableCommand
    {
        EnvironmentVariable EnvironmentVariable { get; set; }
        void Execute();
        Task ExecuteAsync(IProgress<CmdExecutorProgress> progress, float percentage, CancellationToken cancellationToken);
    }
}