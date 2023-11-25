using System;
using ChamiUI.PresentationLayer.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ChamiUI.BusinessLayer.AppLoader.Factories;

internal static class RunningApplicationDetectorFactory
{
    internal static RunningApplicationDetector BuildRunningApplicationDetector(IServiceProvider sp)
    {
        var settingsViewModel = sp.GetRequiredService<SettingsViewModel>();
        var watchedApplications = settingsViewModel.WatchedApplicationSettings.WatchedApplications;
        return new RunningApplicationDetector(watchedApplications);
    }
}