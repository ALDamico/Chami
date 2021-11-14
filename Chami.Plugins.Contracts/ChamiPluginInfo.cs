using System;
using Chami.Plugins.Contracts.ViewModels;

namespace Chami.Plugins.Contracts
{
    public class ChamiPluginInfo
    {
        public string Author { get; set; }
        public Version Version { get; set; }
        public DateTime CreationDate { get; set; }
        public string PluginName { get; set; }

        //TODO This is probably a terrible idea.
        public ChamiPluginInfoViewModel ToViewModel()
        {
            return new ChamiPluginInfoViewModel()
            {
                Author = Author,
                Version = Version.ToString(),
                CreationDate = CreationDate,
                PluginName = PluginName,
                IsEnabled = true
            };
        }
    }
}