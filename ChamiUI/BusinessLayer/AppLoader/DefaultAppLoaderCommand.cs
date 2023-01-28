using System;
using Microsoft.Extensions.DependencyInjection;

namespace ChamiUI.BusinessLayer.AppLoader;

public class DefaultAppLoaderCommand : IAppLoaderCommand
{
    public DefaultAppLoaderCommand(Action<IServiceCollection> actionToExecute, string message)
    {
        ActionToExecute = actionToExecute;
        Message = message;
    }
    public Action<IServiceCollection> ActionToExecute { get; set; }
    public string Message { get; set; }
}