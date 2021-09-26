using System;
using System.Threading;
using System.Threading.Tasks;
using Chami.CmdExecutor;
using Chami.CmdExecutor.Progress;
using Chami.Db.Entities;

namespace ChamiUI.BusinessLayer
{
    public class NopCommand : IShellCommand
    {
        public NopCommand(string customMessage)
        {
            _customMessage = customMessage;
        }  
        public void Execute()
        {
            
        }

        public async Task ExecuteAsync(float percentage, CancellationToken cancellationToken)
        {
            Progress.Report(new CmdExecutorProgress(percentage, _customMessage));
        }

        public IProgress<CmdExecutorProgress> Progress { get; set; }

        private string _customMessage;

        public void SetCustomMessage(string customMessage)
        {
            _customMessage = customMessage;
        }
    }
}