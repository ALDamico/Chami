using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Chami.Plugins.Contracts;
using ChamiUI.Localization;
using McMPlugLoader = McMaster.NETCore.Plugins.PluginLoader;

namespace ChamiUI.BusinessLayer.PluginLoader
{
    public class ChamiPluginLoader
    {
        public ChamiPluginLoader(string applicationPath)
        {
            _applicationPath = applicationPath;
        }

        private string _applicationPath;

        public IEnumerable<IChamiPlugin> LoadPlugins()
        {
            var loaders = new List<McMPlugLoader>();
            var plugins = new List<IChamiPlugin>();
            var pluginsPath = Path.Combine(_applicationPath, "plugins");
            if (!Directory.Exists(pluginsPath))
            {
                throw new ChamiPluginException(ChamiUIStrings.PluginLoadErrorMessage);
            }

            foreach (var directory in Directory.EnumerateDirectories(pluginsPath))
            {
                var directoryBase = Regex.Split(directory, "\\\\|/").Last();
                var dllFileName = Path.Combine(directory, directoryBase + ".dll");
                try
                {
                    var loader =
                        McMPlugLoader.CreateFromAssemblyFile(dllFileName, sharedTypes: new[] { typeof(IChamiPlugin) });
                    loaders.Add(loader);
                }
                catch (Exception ex)
                {
                    var logger = (App.Current as ChamiUI.App).GetLogger();
                    logger.Fatal(ex.Message);
                }
                
                
            }

            foreach (var loader in loaders)
            {
                foreach (var pluginType in loader
                    .LoadDefaultAssembly()
                    .GetTypes()
                    .Where(t => typeof(IChamiPlugin).IsAssignableFrom(t) && !t.IsAbstract))
                {
                    // This assumes the implementation of IPlugin has a parameterless constructor
                    IChamiPlugin plugin = (IChamiPlugin)Activator.CreateInstance(pluginType);

                    var logger = (App.Current as ChamiUI.App).GetLogger();

                    var canLoadPlugin = CanLoadPlugin(plugin);
                    if (logger != null)
                    {
                        var pluginInfo = plugin?.PluginInfo;
                        // TODO Move these strings in resources
                        if (canLoadPlugin)
                        {
                            logger.Information("Loaded plugin {PluginName} version {PluginVersion}",
                                pluginInfo.PluginName, pluginInfo.Version);
                        }
                        else
                        {
                            logger.Warning(
                                "Unable to load the plugin. Either the Plugin Name or its Version are unknown. Plugin Name: {PluginName}, version {PluginVersion}",
                                pluginInfo.PluginName, pluginInfo.Version);
                        }
                    }

                    if (canLoadPlugin)
                    {
                        IChamiPlugin pluginInstance = (IChamiPlugin)Activator.CreateInstance(pluginType);
                        plugins.Add(pluginInstance);
                    }
                }
            }
            return plugins;
        }

        private bool CanLoadPlugin(IChamiPlugin plugin)
        {
            if (plugin == null)
            {
                return false;
            }

            var pluginInfo = plugin.PluginInfo;
            return pluginInfo.PluginName != null && pluginInfo.Version != null;
        }
    }
}