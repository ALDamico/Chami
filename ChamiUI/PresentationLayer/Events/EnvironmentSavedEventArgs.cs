using System;
using ChamiUI.PresentationLayer.ViewModels;

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