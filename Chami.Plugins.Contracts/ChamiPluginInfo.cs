using System;

namespace Chami.Plugins.Contracts
{
    public class ChamiPluginInfo
    {
        public string Author { get; set; }
        public Version Version { get; set; }
        public DateTime CreationDate { get; set; }
        public string PluginName { get; set; }
    }
}