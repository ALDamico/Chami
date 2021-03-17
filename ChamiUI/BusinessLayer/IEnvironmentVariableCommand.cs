using System;
using System.Threading.Tasks;
using ChamiUI.DataLayer.Entities;
using ChamiUI.PresentationLayer.Progress;

namespace ChamiUI.BusinessLayer
{
    public interface IEnvironmentVariableCommand
    {
        EnvironmentVariable EnvironmentVariable { get; set; }
        void Execute();
        Task ExecuteAsync(IProgress<CmdExecutorProgress> progress);
    }
}