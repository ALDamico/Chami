using System.Collections.ObjectModel;
using Chami.Plugins.Contracts;
using Chami.Plugins.Contracts.ViewModels;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class PluginManagerViewModel : ViewModelBase
    {
        public PluginManagerViewModel()
        {
            AvailablePlugins = new ObservableCollection<ChamiPluginInfoViewModel>();
        }
        public ObservableCollection<ChamiPluginInfoViewModel> AvailablePlugins { get;  }
        public ChamiPluginInfoViewModel SelectedPlugin { get; set; }
        public ObservableCollection<ChamiPluginInfoViewModel> SelectedPlugins { get; set; }

        public void DisableSelectedPlugin()
        {
            (App.Current as ChamiUI.App).LoadedPlugins.RemoveAll(p =>
                p.PluginInfo.InstanceGuid == SelectedPlugin.InstanceGuid);
            SelectedPlugin.IsEnabled = false;
        }
    }
}