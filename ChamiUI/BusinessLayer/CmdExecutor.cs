using ChamiUI.PresentationLayer.Progress;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChamiUI.BusinessLayer
{
    public class CmdExecutor
    {
        public CmdExecutor()
        {
            EnvironmentVariablesToApply = new List<IEnvironmentVariableCommand>();
        }

        public void AddCommand(IEnvironmentVariableCommand command)
        {
            EnvironmentVariablesToApply.Add(command);
        }
        protected List<IEnvironmentVariableCommand> EnvironmentVariablesToApply { get; }

        public void Execute()
        {
            foreach (var environmentVariable in EnvironmentVariablesToApply)
            {
                environmentVariable.Execute();
            }
        }

        public async Task ExecuteAsync(IProgress<CmdExecutorProgress> progress)
        {
            CmdExecutorProgress cmdExecutorProgress = new CmdExecutorProgress(0, null, "Starting execution...\n");
            progress?.Report(cmdExecutorProgress);
            var tasks = new List<Task>();
            foreach (var environmentVariable in EnvironmentVariablesToApply)
            {
                await environmentVariable.ExecuteAsync(progress);
            }

            progress?.Report(new CmdExecutorProgress(100, null, "Execution complete\n"));
        }
    }
}