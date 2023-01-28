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
        _postServiceProviderBuildCommands = new List<IAppLoaderCommand>();
        _progress = new Progress<AppLoadProgress>(progressHandler);
        _serviceCollection = new ServiceCollection();
    }

    public AppLoader AddCommand(IAppLoaderCommand command, bool isPostBuildAction = false)
    {
        if (isPostBuildAction)
        {
            _postServiceProviderBuildCommands.Add(command);
        }
        else
        {
            _commands.Add(command);
        }

        return this;
    }

    public Task<IServiceProvider> ExecuteAsync()
    {
        int numberOfCommands = _commands.Count + _postServiceProviderBuildCommands.Count;

        _commands.Select((c, i) => new { Command = c, Percentage = (i + 1) * 100 / numberOfCommands }).ToList().ForEach(async c =>
        {
            _progress.Report(new AppLoadProgress() { Message = c.Command.Message, Percentage = c.Percentage });
            await c.Command.ActionToExecute(_serviceCollection);
        });
        var serviceProvider = _serviceCollection.BuildServiceProvider();
        
        return Task.FromResult((IServiceProvider)serviceProvider);
    }

    public Task ExecutePostBuildCommandsAsync()
    {
        int numberOfCommands = _commands.Count + _postServiceProviderBuildCommands.Count;
        int alreadyExecutedCommands = _commands.Count;

        _postServiceProviderBuildCommands.Select((c, i) => new { Command = c, Percentage = (i + 1 + alreadyExecutedCommands) * 100 / numberOfCommands })
            .ToList().ForEach(async c =>
            {
                _progress.Report(new AppLoadProgress() { Message = c.Command.Message, Percentage = c.Percentage });
                var action = c.Command.ActionToExecute;

                await action(_serviceCollection);
            });
        
        return Task.CompletedTask;
    }

    private readonly List<IAppLoaderCommand> _commands;
    private readonly List<IAppLoaderCommand> _postServiceProviderBuildCommands;
    private readonly IProgress<AppLoadProgress> _progress;
    private readonly ServiceCollection _serviceCollection;
}