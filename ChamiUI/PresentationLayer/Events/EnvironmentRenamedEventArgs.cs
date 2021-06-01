using System;

namespace ChamiUI.PresentationLayer.Events
{
    public class EnvironmentRenamedEventArgs:EventArgs
    {
        public EnvironmentRenamedEventArgs(string newName)
        {
            NewName = newName;
        }
        public string NewName { get; }
        
    }
}