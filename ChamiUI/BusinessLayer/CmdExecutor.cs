using ChamiUI.PresentationLayer.Progress;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChamiUI.BusinessLayer.Converters;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer
{
    public class CmdExecutor
    {
        public CmdExecutor()
        {
            EnvironmentVariablesToApply = new List<IEnvironmentVariableCommand>();
        }

        public CmdExecutor(DataLayer.Entities.Environment targetEnvironment) : this()
        {
            TargetEnvironment = targetEnvironment;
        }

        public CmdExecutor(EnvironmentViewModel targetEnvironmentViewModel) : this()
        {
            var converter = new EnvironmentConverter();
            var convertedEnvironment = converter.From(targetEnvironmentViewModel);
            TargetEnvironment = convertedEnvironment;
        }
        
        public DataLayer.Entities.Environment TargetEnvironment { get; }

        public void AddCommand(IEnvironmentVariableCommand command)
        {
            EnvironmentVariablesToApply.Add(command);
        }

        public event EventHandler<EnvironmentChangedEventArgs> EnvironmentChanged;

        protected virtual void OnEnvironmentChanged(object sender, EnvironmentChangedEventArgs args)
        {
            EnvironmentChanged?.Invoke(this, args);
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
            foreach (var environmentVariable in EnvironmentVariablesToApply)
            {
                await environmentVariable.ExecuteAsync(progress);
            }

            progress?.Report(new CmdExecutorProgress(100, null, "Execution complete\n"));
            if (TargetEnvironment != null)
            {
                var converter = new EnvironmentConverter();
                var convertedViewModel = converter.To(TargetEnvironment);
                OnEnvironmentChanged(this, new EnvironmentChangedEventArgs(convertedViewModel));
            }
            
        }
    }
}