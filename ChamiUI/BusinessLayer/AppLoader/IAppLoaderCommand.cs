using System;
using Microsoft.Extensions.DependencyInjection;

namespace ChamiUI.BusinessLayer.AppLoader;

public interface IAppLoaderCommand
{
    Action<IServiceCollection> ActionToExecute { get; set; }
    string Message { get; set; }
}