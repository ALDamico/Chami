using System;
using Chami.Db.Entities;
using ChamiUI.BusinessLayer.Converters;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.PresentationLayer.Utils
{
    /// <summary>
    /// Helper class for getting the settings from the current application object.
    /// </summary>
    public static class SettingsUtils
    {
        /// <summary>
        /// Gets the settings from the current application.
        /// </summary>
        /// <returns>A <see cref="SettingsViewModel"/> object containing the current settings.</returns>
        public static SettingsViewModel GetAppSettings()
        {
            return (System.Windows.Application.Current as App)?.Settings;
        }

        public static ISettingConverter<T> GetConverter<T>(Setting setting)
        {
            return (ISettingConverter<T>) Activator.CreateInstance(null, setting.Converter)?.Unwrap();
        }
    }
}
