using ChamiUI.PresentationLayer.Progress;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ChamiUI.BusinessLayer.Converters;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;
using ChamiUI.Windows.MainWindow;
using WPFLocalizeExtension.Providers;
using Environment = Chami.Db.Entities.Environment;

namespace ChamiUI.BusinessLayer
{
    /// <summary>
    /// Executes sequences of Windows shells commands.
    /// </summary>
    public class CmdExecutor
    {
        /// <summary>
        /// Constructs a new CmdExecutor object and initializes the command queue.
        /// </summary>
        public CmdExecutor()
        {
            EnvironmentVariablesToApply = new List<IEnvironmentVariableCommand>();
        }

        /// <summary>
        /// Constructs a new <see cref="CmdExecutor"/> object and sets the <see cref="TargetEnvironment"/> property.
        /// </summary>
        /// <param name="targetEnvironment">The environment that is being targeted.</param>
        public CmdExecutor(Environment targetEnvironment) : this()
        {
            TargetEnvironment = targetEnvironment;
        }

        /// <summary>
        /// Constructs a new <see cref="CmdExecutor"/> object and sets the <see cref="TargetEnvironment"/> property.
        /// </summary>
        /// <param name="targetEnvironmentViewModel">The environment that is being targeted.</param>
        public CmdExecutor(EnvironmentViewModel targetEnvironmentViewModel) : this()
        {
            var converter = new EnvironmentConverter();
            var convertedEnvironment = converter.From(targetEnvironmentViewModel);
            TargetEnvironment = convertedEnvironment;
        }
        
        /// <summary>
        /// The environment to target.
        /// </summary>
        public Environment TargetEnvironment { get; }

        /// <summary>
        /// Adds a new <see cref="IEnvironmentVariableCommand"/> to the command queue.
        /// </summary>
        /// <param name="command">The command to perform when the <see cref="Execute"/> or <see cref="ExecuteAsync"/> methods are invoked.</param>
        public void AddCommand(IEnvironmentVariableCommand command)
        {
            EnvironmentVariablesToApply.Add(command);
        }

        /// <summary>
        /// An event that gets fired when an the target environment changes.
        /// </summary>
        public event EventHandler<EnvironmentChangedEventArgs> EnvironmentChanged;

        /// <summary>
        /// Event handler for the <see cref="EnvironmentChanged"/> event.
        /// </summary>
        /// <param name="sender">The object that sends the request. Unused.</param>
        /// <param name="args">Information about the environment that has changed.</param>
        protected virtual void OnEnvironmentChanged(object sender, EnvironmentChangedEventArgs args)
        {
            EnvironmentChanged?.Invoke(this, args);
        }

        /// <summary>
        /// The queue of commands to execute.
        /// </summary>
        protected List<IEnvironmentVariableCommand> EnvironmentVariablesToApply { get; }

        /// <summary>
        /// Performs all the commands in the <see cref="EnvironmentVariablesToApply"/> queue synchronously.
        /// </summary>
        public void Execute()
        {
            foreach (var environmentVariable in EnvironmentVariablesToApply)
            {
                environmentVariable.Execute();
            }
        }

        /// <summary>
        /// Performs all the commands in the <see cref="EnvironmentVariablesToApply"/> queue asynchronously.
        /// It optionally notifies of progress and execution can be canceled.
        /// </summary>
        /// <param name="progress">Notifies of progress.</param>
        /// <param name="cancellationToken">Allows canceling the execution midway and revert to a previous state.</param>
        public async Task ExecuteAsync(IProgress<CmdExecutorProgress> progress, CancellationToken cancellationToken)
        {
            var message = ChamiUIStrings.StartingExecutionMessage;
            CmdExecutorProgress cmdExecutorProgress = new CmdExecutorProgress(0, null,  message);
            progress?.Report(cmdExecutorProgress);
            foreach (var environmentVariable in EnvironmentVariablesToApply)
            {
                var currentIndex = (float)EnvironmentVariablesToApply.IndexOf(environmentVariable);
                float percentage = 100.0F * currentIndex / EnvironmentVariablesToApply.Count;
                await environmentVariable.ExecuteAsync(progress, percentage, cancellationToken);
            }

            progress?.Report(new CmdExecutorProgress(100, null, ChamiUIStrings.ExecutionCompleteMessage));
            if (TargetEnvironment != null)
            {
                var converter = new EnvironmentConverter();
                var convertedViewModel = converter.To(TargetEnvironment);
                OnEnvironmentChanged(this, new EnvironmentChangedEventArgs(convertedViewModel));
            }
            
        }
    }
}