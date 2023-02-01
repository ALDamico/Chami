using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ChamiUI.BusinessLayer.AppLoader;

public interface IAppLoaderCommand
{
    Func<IServiceCollection, Task> ActionToExecute { get; set; }
    Func<IServiceProvider, Task> PostActionToExecute { get; set; }
    string Message { get; set; }
    string Name => ActionToExecute != null ? ActionToExecute.Method.Name : PostActionToExecute.Method.Name;
}