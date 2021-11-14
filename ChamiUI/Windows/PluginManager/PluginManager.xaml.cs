using System.Windows;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Windows.PluginManager
{
    public partial class PluginManager : Window
    {
        private PluginManagerViewModel _viewModel;
        public PluginManager()
        {
            _viewModel = new PluginManagerViewModel();
            DataContext = _viewModel;
            InitializeComponent();
        }

        private void PluginManager_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                var loadedPlugins = (Application.Current as ChamiUI.App)?.LoadedPlugins;
                if (loadedPlugins == null)
                {
                    return;
                }
                foreach (var plugin in loadedPlugins)
                {
                    _viewModel.AvailablePlugins.Add(plugin.PluginInfo.ToViewModel());
                }
                ;
            }
            
        }
    }
}