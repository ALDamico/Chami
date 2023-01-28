using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChamiUI.PresentationLayer.Progress;
using ChamiUI.Windows.EnvironmentHealth;
using Microsoft.Extensions.DependencyInjection;

namespace ChamiUI.BusinessLayer.AppLoader;

public class AppLoader
{
    public AppLoader(Action<AppLoadProgress> progressHandler)
    {
        _commands = new List<IAppLoaderCommand>();
        _progress = new Progress<AppLoadProgress>(progressHandler);
        _serviceCollection = new ServiceCollection();
    }

    public AppLoader AddCommand(IAppLoaderCommand command)
    {
        _commands.Add(command);
        return this;
    }

    public Task<IServiceProvider> ExecuteAsync()
    {
        int numberOfCommands = _commands.Count;

        _commands.Select((c, i) => new {Command = c, Percentage = i * 100 / numberOfCommands}).ToList().ForEach(c =>
        {
            _progress.Report(new AppLoadProgress(){Message = c.Command.Message, Percentage = c.Percentage});
            c.Command.ActionToExecute(_serviceCollection);
        });
        return Task.FromResult(_serviceCollection.BuildServiceProvider() as IServiceProvider);
    }
    private readonly List<IAppLoaderCommand> _commands;
    private readonly IProgress<AppLoadProgress> _progress;
    private readonly ServiceCollection _serviceCollection;
}