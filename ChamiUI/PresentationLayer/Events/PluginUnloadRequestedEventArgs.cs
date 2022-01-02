using System;
using Chami.Plugins.Contracts;

namespace ChamiUI.PresentationLayer.Events
{
    public class PluginUnloadRequestedEventArgs : EventArgs
    {
        public PluginUnloadRequestedEventArgs(Guid instanceGuid)
        {
            InstanceGuid = instanceGuid;
        }
        
        public Guid InstanceGuid { get; }
    }
}