using System.IO;
using ChamiUI.BusinessLayer;

namespace ChamiUI.PresentationLayer.Progress
{
    /// <summary>
    /// Notifies progress of a <see cref="CmdExecutorBase"/>.
    /// Notification messages can be <see cref="Stream"/>s or strings.
    /// </summary>
    /// <seealso cref="CmdExecutorBase"/>
    public readonly struct CmdExecutorProgress
    {
        public CmdExecutorProgress(float percentage, Stream outputStream, string message)
        {
            Percentage = percentage;
            OutputStream = outputStream;
            Message = message;
        }
        /// <summary>
        /// The execution percentage. No special checks are made to ensure that this property isn't greater than 100.
        /// </summary>
        public float Percentage { get; }
        
        /// <summary>
        /// A <see cref="Stream"/> object that we want to include in the progress reporting. It can be, for example, the
        /// standard output of a shell command.
        /// </summary>
        public Stream OutputStream { get; }
        
        /// <summary>
        /// A programmer-defined message included in the progress reporting.
        /// </summary>
        public string Message { get; }
    }
}