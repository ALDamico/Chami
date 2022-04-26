using System.Collections.ObjectModel;
using System.Windows.Media;
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
        
        /// <summary>
        /// Retrieves all font families installed on this machine.
        /// </summary>
        /// <returns>An <see cref="ObservableCollection{T}"/> containing all installed fonts in the current machine.</returns>
        public static ObservableCollection<FontFamily> GetInstalledFonts()
        {
            return new ObservableCollection<FontFamily>(Fonts.GetFontFamilies("c:/windows/fonts"));
        }
    }
}
