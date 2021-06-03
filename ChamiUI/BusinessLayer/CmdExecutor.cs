using ChamiUI.PresentationLayer.Progress;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ChamiUI.BusinessLayer.Converters;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;
using ChamiUI.Windows.MainWindow;
using WPFLocalizeExtension.Providers;

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
            var message = ChamiUIStrings.StartingExecutionMessage;
            CmdExecutorProgress cmdExecutorProgress = new CmdExecutorProgress(0, null,  message);
            progress?.Report(cmdExecutorProgress);
            foreach (var environmentVariable in EnvironmentVariablesToApply)
            {
                var currentIndex = (float)EnvironmentVariablesToApply.IndexOf(environmentVariable);
                float percentage = 100.0F * currentIndex / EnvironmentVariablesToApply.Count;
                await environmentVariable.ExecuteAsync(progress, percentage);
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