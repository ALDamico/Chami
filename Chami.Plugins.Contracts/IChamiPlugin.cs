using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Chami.Plugins.Contracts.ViewModels;
using FluentMigrator;

namespace Chami.Plugins.Contracts
{
    public interface IChamiPlugin
    {
        TabbedControlViewModel PluginInterface { get; }
        ChamiPluginInfo PluginInfo { get; }
        Window GetPluginWindow();
        ToolBar GetPluginToolbar();
    }
}