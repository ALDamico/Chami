using Chami.Plugins.Contracts.ViewModels;

namespace Chami.Plugins.Contracts
{
    public interface IChamiPlugin
    {
        TabbedControlViewModel PluginInterface { get; }
        ChamiPluginInfo PluginInfo { get; }
    }
}