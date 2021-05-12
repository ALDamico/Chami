using System;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.PresentationLayer.Events
{
    public class EnvironmentChangedEventArgs : EventArgs
    {
        public EnvironmentChangedEventArgs(EnvironmentViewModel newActiveEnvironment)
        {
            NewActiveEnvironment = newActiveEnvironment;
        }
        public EnvironmentViewModel NewActiveEnvironment { get; }
    }
}