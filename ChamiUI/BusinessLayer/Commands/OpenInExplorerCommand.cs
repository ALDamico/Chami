using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Chami.CmdExecutor;

namespace ChamiUI.BusinessLayer.Commands
{
    public class OpenInExplorerCommand : ShellCommandBase
    {
        private string _path;
        public OpenInExplorerCommand(string path)
        {
            _path = path;
        }
        public override void Execute()
        {
            var process = PrepareProcess(_path);
            process.StartInfo.CreateNoWindow = false;
            process.Start();
        }

        public override Task ExecuteAsync(float percentage, CancellationToken cancellationToken)
        {
            throw new System.NotSupportedException();
        }

        protected override Process PrepareProcess(string arguments)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo("explorer", arguments)
            {
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
            Process process = new Process()
            {
                StartInfo = processStartInfo
            };
            return process;
        }
    }
}