using System;
using System.Linq;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.BusinessLayer.Services;
using ChamiUI.PresentationLayer.Minimizing;
using ChamiUI.PresentationLayer.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace ChamiUI.BusinessLayer.AppLoader.Factories;

public class MinimizationServiceFactory
{
    public static MinimizationService BuildMinimizationService(IServiceProvider serviceProvider)
    {
        var service = new MinimizationService();
        service.AvailableStrategies.Add(MinimizeToTaskbarStrategy.Instance);
        service.AvailableStrategies.Add(MinimizeToTrayStrategy.Instance);
        var dataAdapter = serviceProvider.GetRequiredService<SettingsDataAdapter>();
        var settingsList = dataAdapter.GetSettingsList();
        var setting = settingsList.FirstOrDefault(s => s.Type.Equals(typeof(IMinimizationStrategy).FullName));
        var converter = SettingsUtils.GetConverter<IMinimizationStrategy>(setting);
        var converted = converter.Convert(setting);
        service.MinimizationStrategy = converted;
        return service;
    }
}