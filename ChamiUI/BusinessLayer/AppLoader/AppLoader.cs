using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChamiUI.PresentationLayer.Progress;
using ChamiUI.Windows.EnvironmentHealth;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

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

    public AppLoader AddCommand(IAppLoaderCommand command)
    {
        _commands.Add(command);

        return this;
    }

    public AppLoader AddPostBuildCommand(IAppLoaderCommand command)
    {
        _postServiceProviderBuildCommands.Add(command);
        return this;
    }

    public async Task<IServiceProvider> ExecuteAsync()
    {
        int numberOfCommands = _commands.Count + _postServiceProviderBuildCommands.Count;

        await Task.Run(() => _commands
            .Select((c, i) => new {Command = c, Percentage = (i + 1) * 100 / numberOfCommands}).ToList().ForEach(
                async c =>
                {
                    var when = DateTime.Now;
                    _progress.Report(new AppLoadProgress() {Message = c.Command.Message, Percentage = c.Percentage});
                    await c.Command.ActionToExecute(_serviceCollection);

                    var taken = (DateTime.Now - when).TotalMilliseconds;
                    Log.Information("Service {Name} registered in {TimeTaken} milliseconds", c.Command.Name, taken);
                }));
        _serviceProvider = _serviceCollection.BuildServiceProvider();

        return _serviceProvider;
    }

    public async Task ExecutePostBuildCommandsAsync()
    {
        int numberOfCommands = _commands.Count + _postServiceProviderBuildCommands.Count;
        int alreadyExecutedCommands = _commands.Count;

        await Task.Run(() => _postServiceProviderBuildCommands.Select((c, i) =>
                new {Command = c, Percentage = (i + 1 + alreadyExecutedCommands) * 100 / numberOfCommands})
            .ToList().ForEach(async c =>
            {
                var when = DateTime.Now;
                _progress.Report(new AppLoadProgress() {Message = c.Command.Message, Percentage = c.Percentage});
                var action = c.Command.PostActionToExecute;

                await action(_serviceProvider);
                var taken = (DateTime.Now - when).TotalMilliseconds;
                Log.Information("Service {Name} registered in {TimeTaken} milliseconds", c.Command.Name, taken);
            }));
    }

    private readonly List<IAppLoaderCommand> _commands;
    private readonly List<IAppLoaderCommand> _postServiceProviderBuildCommands;
    private readonly IProgress<AppLoadProgress> _progress;
    private readonly ServiceCollection _serviceCollection;
    private ServiceProvider _serviceProvider;
}