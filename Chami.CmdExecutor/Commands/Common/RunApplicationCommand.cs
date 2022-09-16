using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Chami.CmdExecutor;

namespace ChamiUI.BusinessLayer.Commands;

public class RunApplicationCommand : OpenInExplorerCommand
{
    public override void Execute()
    {
        if (!File.Exists(Path))
        {
            throw new InvalidOperationException();
        }
        
        
        
        base.Execute();
    }

    public override Task ExecuteAsync(float percentage, CancellationToken cancellationToken)
    {
        throw new System.NotSupportedException();
    }

    public RunApplicationCommand(string path) : base(path)
    {
    }
}