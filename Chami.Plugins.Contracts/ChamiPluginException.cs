using System;

namespace Chami.Plugins.Contracts
{
    public class ChamiPluginException : Exception
    {
        public ChamiPluginException(string message) : base(message)
        {
            
        }

        public ChamiPluginException()
        {
            
        }
    }
}