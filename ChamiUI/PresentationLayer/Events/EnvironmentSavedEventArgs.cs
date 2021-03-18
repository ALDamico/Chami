using System;

namespace ChamiUI.PresentationLayer.Events
{
    public class EnvironmentSavedEventArgs:EventArgs
    {
        public EnvironmentSavedEventArgs(EnvironmentViewModel environmentViewModel)
        {
            EnvironmentViewModel = environmentViewModel;
        }
        public EnvironmentViewModel EnvironmentViewModel { get; }
    }
}