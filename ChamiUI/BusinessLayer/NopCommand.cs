using System;
using System.Threading;
using System.Threading.Tasks;
using Chami.CmdExecutor;
using Chami.CmdExecutor.Progress;
using Chami.Db.Entities;

namespace ChamiUI.BusinessLayer
{
    /// <summary>
    /// Represent a no-op command for use by <see cref="CmdExecutorBase"/> and its derived classes
    /// </summary>
    public class NopCommand : IShellCommand
    {
        /// <summary>
        /// Constructs a new <see cref="NopCommand"/> object and sets its custom message.
        /// </summary>
        /// <param name="customMessage"></param>
        public NopCommand(string customMessage)
        {
            _customMessage = customMessage;
        }  
        
        /// <summary>
        /// NO-OP
        /// </summary>
        public void Execute()
        {
            
        }

        /// <summary>
        /// Reports the custom message.
        /// </summary>
        /// <param name="percentage">The percentage completion.</param>
        /// <param name="cancellationToken">Unused. Can be null or <see cref="CancellationToken.None"/>.</param>
        public async Task ExecuteAsync(float percentage, CancellationToken cancellationToken)
        {
            Progress.Report(new CmdExecutorProgress(percentage, _customMessage));
            await Task.CompletedTask;
        }

        /// <summary>
        /// Handles progress reporting.
        /// </summary>
        public IProgress<CmdExecutorProgress> Progress { get; set; }

        private string _customMessage;

        /// <summary>
        /// Sets a new custom message.
        /// </summary>
        /// <param name="customMessage">The new custom message.</param>
        public void SetCustomMessage(string customMessage)
        {
            _customMessage = customMessage;
        }
    }
}