using System;
using System.Collections.Generic;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.PresentationLayer.Events
{
    public class ApplicationsDetectedEventArgs: EventArgs
    {
        public ApplicationsDetectedEventArgs(List<WatchedApplicationViewModel> watchedApplicationViewModels)
        {
            DetectedApplications = watchedApplicationViewModels;
        }
        
        public List<WatchedApplicationViewModel> DetectedApplications { get; }
    }
}