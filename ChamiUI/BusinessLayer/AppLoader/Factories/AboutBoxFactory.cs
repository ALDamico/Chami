using System;
using ChamiUI.Windows.AboutBox;
using ChamiUI.Windows.MainWindow;
using Microsoft.Extensions.DependencyInjection;

namespace ChamiUI.BusinessLayer.AppLoader.Factories;

internal static class AboutBoxFactory
{
    internal static AboutBox BuildAboutBox(IServiceProvider sp)
    {
        return new AboutBox(sp.GetRequiredService<MainWindow>());
    }
}