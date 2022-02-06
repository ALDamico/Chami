using System.IO;

namespace ChamiUI.PresentationLayer.Progress
{
    public struct CmdExecutorProgress
    {
        public CmdExecutorProgress(float percentage, Stream outputStream, string message)
        {
            Percentage = percentage;
            OutputStream = outputStream;
            Message = message;
        }
        public float Percentage { get; }
        public Stream OutputStream { get; }
        public string Message { get; }
    }
}