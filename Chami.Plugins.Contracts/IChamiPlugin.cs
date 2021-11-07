using System.Collections.Generic;
using Chami.Plugins.Contracts.ViewModels;
using FluentMigrator;

namespace Chami.Plugins.Contracts
{
    public interface IChamiPlugin
    {
        TabbedControlViewModel PluginInterface { get; }
        ChamiPluginInfo PluginInfo { get; }
        List<Migration> PluginMigrations { get; }
    }
}