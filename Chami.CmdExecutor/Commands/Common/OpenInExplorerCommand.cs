using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Chami.CmdExecutor.Commands.Common
{
    public class OpenInExplorerCommand : ShellCommandBase
    {
        protected readonly string Path;
        public OpenInExplorerCommand(string path)
        {
            Path = path;
        }
        public override void Execute()
        {
            var process = PrepareProcess(Path);
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