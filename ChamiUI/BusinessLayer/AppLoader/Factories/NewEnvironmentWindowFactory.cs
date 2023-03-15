using System;
using ChamiUI.PresentationLayer.ViewModels;
using ChamiUI.Windows.MainWindow;
using ChamiUI.Windows.NewEnvironmentWindow;
using Microsoft.Extensions.DependencyInjection;

namespace ChamiUI.BusinessLayer.AppLoader.Factories;

internal static class NewEnvironmentWindowFactory
{
    internal static NewEnvironmentWindow BuildNewEnvironmentWindow(IServiceProvider sp)
    {
        return new NewEnvironmentWindow(sp.GetRequiredService<MainWindow>(),
            sp.GetService<NewEnvironmentViewModel>());
    }
}