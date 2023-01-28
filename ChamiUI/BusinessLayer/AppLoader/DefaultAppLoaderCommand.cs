using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ChamiUI.BusinessLayer.AppLoader;

public class DefaultAppLoaderCommand : IAppLoaderCommand
{
    public DefaultAppLoaderCommand(Func<IServiceCollection, Task> actionToExecute, string message)
    {
        ActionToExecute = actionToExecute;
        Message = message;
    }
    public Func<IServiceCollection, Task> ActionToExecute { get; set; }
    public string Message { get; set; }
}