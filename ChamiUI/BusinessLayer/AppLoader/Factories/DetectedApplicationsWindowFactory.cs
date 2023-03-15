using System;
using ChamiUI.PresentationLayer.ViewModels;
using ChamiUI.Windows.DetectedApplicationsWindow;
using ChamiUI.Windows.MainWindow;
using Microsoft.Extensions.DependencyInjection;

namespace ChamiUI.BusinessLayer.AppLoader.Factories;

internal static class DetectedApplicationsWindowFactory
{
    internal static DetectedApplicationsWindow BuildDetectedApplicationsWindow(IServiceProvider sp)
    {
        var window = new DetectedApplicationsWindow(sp.GetRequiredService<DetectedApplicationsViewModel>());
        window.Owner = sp.GetRequiredService<MainWindow>();
        return window;
    }
}