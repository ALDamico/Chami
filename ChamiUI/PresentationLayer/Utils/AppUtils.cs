using ChamiUI.Windows.SettingsWindow;
using Microsoft.Extensions.DependencyInjection;

namespace ChamiUI.PresentationLayer.Utils;

public static class AppUtils
{
    public static SettingsWindow GetSettingsWindow(App app)
    {
        return app.ServiceProvider.GetRequiredService<SettingsWindow>();
    }
}