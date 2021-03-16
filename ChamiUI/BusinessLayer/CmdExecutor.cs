using System.Collections.Generic;
using System.Diagnostics;

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
    }
}