using ChamiUI.PresentationLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
