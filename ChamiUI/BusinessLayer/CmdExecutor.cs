using System;
using System.Threading;
using System.Threading.Tasks;
using Chami.CmdExecutor;
using Chami.CmdExecutor.Progress;
using ChamiUI.BusinessLayer.Converters;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;
using Environment = Chami.Db.Entities.Environment;

namespace ChamiUI.BusinessLayer
{
    /// <summary>
    /// Executes groups of command prompt commands and emits events that the Chami user interface can react to.
    /// </summary>
    /// <seealso cref="CmdExecutorBase"/>
    public class CmdExecutor : CmdExecutorBase
    {
        /// <summary>
        /// Constructs a new <see cref="CmdExecutor"/> object and initializes its command list to an empty list.
        /// </summary>
        /// <seealso cref="CmdExecutorBase"/>
        public CmdExecutor()
        {
        }

        /// <summary>
        /// Constructs a new <see cref="CmdExecutor"/> object, initializes its command list and sets the <see cref="TargetEnvironment"/> property.
        /// </summary>
        /// <param name="targetEnvironment">The target environment which will participate in the <see cref="EnvironmentChanged"/> event.</param>
        /// <seealso cref="CmdExecutorBase"/>
        public CmdExecutor(Environment targetEnvironment)
        {
            TargetEnvironment = targetEnvironment;
        }

        /// <summary>
        /// Constructs a new <see cref="CmdExecutor"/> object, initializes its command queue and sets the <see cref="TargetEnvironment"/> property.
        /// </summary>
        /// <param name="targetEnvironmentViewModel">The target environment which will participate in the <see cref="EnvironmentChanged"/> event.</param>
        /// <seealso cref="CmdExecutorBase"/>
        public CmdExecutor(EnvironmentViewModel targetEnvironmentViewModel)
        {
            var converter = new EnvironmentConverter();
            var convertedEnvironment = converter.From(targetEnvironmentViewModel);
            TargetEnvironment = convertedEnvironment;
        }

        /// <summary>
        /// The environment to target.
        /// </summary>
        public Environment TargetEnvironment { get; }

        public event EventHandler<EnvironmentChangedEventArgs> EnvironmentChanged;

        /// <summary>
        /// Event handler for the <see cref="EnvironmentChanged"/> event.
        /// </summary>
        /// <param name="sender">The object that sends the request. Unused.</param>
        /// <param name="args">Information about the environment that has changed.</param>
        protected virtual void OnEnvironmentChanged(EnvironmentChangedEventArgs args)
        {
            EnvironmentChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Performs all the commands in the <see cref="CmdExecutorBase.CommandQueue"/> queue asynchronously.
        /// It optionally notifies of progress and execution can be canceled.
        /// </summary>
        /// <param name="progress">Notifies of progress.</param>
        /// <param name="cancellationToken">Allows canceling the execution midway and revert to a previous state.</param>
        public override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            
            await base.ExecuteAsync(cancellationToken);
            if (TargetEnvironment != null)
            {
                var converter = new EnvironmentConverter();
                var convertedViewModel = converter.To(TargetEnvironment);
                OnEnvironmentChanged(new EnvironmentChangedEventArgs(convertedViewModel));
            }
        }
    }
}