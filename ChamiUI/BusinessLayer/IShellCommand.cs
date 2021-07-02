using ChamiUI.PresentationLayer.Progress;
using System;
using System.Threading;
using System.Threading.Tasks;
using Chami.Db.Entities;

namespace ChamiUI.BusinessLayer
{
    public interface IShellCommand
    {
        EnvironmentVariable EnvironmentVariable { get; set; }
        void Execute();
        Task ExecuteAsync(IProgress<CmdExecutorProgress> progress, float percentage, CancellationToken cancellationToken);
    }
}