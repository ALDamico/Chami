using System;

namespace ChamiUI.PresentationLayer.Events
{
    public class EnvironmentExistingEventArgs:EventArgs
    {
        public EnvironmentExistingEventArgs(string name)
        {
            Name = name;
        }
        public bool Exists
        {
            get => true;
        }
        public string Name { get; set; }
    }
}