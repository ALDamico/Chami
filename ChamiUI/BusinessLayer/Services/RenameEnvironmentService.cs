using System;
using System.Threading.Tasks;
using ChamiUI.PresentationLayer.Events;

namespace ChamiUI.BusinessLayer.Services;

public class RenameEnvironmentService
{
    public event EventHandler<EnvironmentRenamedEventArgs> EnvironmentRenamed;
    
    protected virtual void OnEnvironmentRenamed(string newName)
    {
        EnvironmentRenamed?.Invoke(this, new EnvironmentRenamedEventArgs(newName));
    }

    public async Task RenameEnvironment(string name)
    {
        OnEnvironmentRenamed(name);
        await Task.CompletedTask;
    }
}