using System;
using Microsoft.Extensions.DependencyInjection;

namespace ChamiUI.BusinessLayer.AppLoader;

public sealed class BuildServiceProviderLoaderCommand : IAppLoaderCommand
{
    public BuildServiceProviderLoaderCommand()
    {
        ActionToExecute = collection => collection.BuildServiceProvider();
    }
    public Action<IServiceCollection> ActionToExecute { get; set; }
    public string Message { get; set; }
}