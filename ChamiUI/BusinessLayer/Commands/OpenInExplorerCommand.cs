using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Chami.CmdExecutor;

namespace ChamiUI.BusinessLayer.Commands
{
    public class OpenInExplorerCommand : ShellCommandBase
    {
        protected string Path;
        public OpenInExplorerCommand(string path)
        {
            Path = path;
        }
        public override void Execute()
        {
            var process = PrepareProcess(Path);
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
                RedirectStandardOutput = true,
                CreateNoWindow = false
            };
            Process process = new Process()
            {
                StartInfo = processStartInfo
            };
            return process;
        }
    }
}