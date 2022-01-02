using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.PresentationLayer.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;

namespace ChamiUI.BusinessLayer.Factories
{
    /// <summary>
    /// Static factory to construct a fully-functional <see cref="SettingsViewModel"/>.
    /// </summary>
    public static class SettingsViewModelFactory
    {
        /// <summary>
        /// Retrieves all Chami settings.
        /// </summary>
        /// <param name="dataAdapter">The <see cref="SettingsDataAdapter"/> to use to retrieve settings.</param>
        /// <param name="watchedApplicationDataAdapter">The <see cref="WatchedApplicationDataAdapter"/> to retrieve the <see cref="WatchedApplicationViewModel"/>s.</param>
        /// <param name="languageDataAdapter">The <see cref="ApplicationLanguageDataAdapter"/> to retrieve all <see cref="ApplicationLanguageViewModel"/>s.</param>
        /// <returns>A <see cref="SettingsViewModel"/> object.</returns>
        public static SettingsViewModel GetSettings(SettingsDataAdapter dataAdapter, WatchedApplicationDataAdapter watchedApplicationDataAdapter, ApplicationLanguageDataAdapter languageDataAdapter)
        {
            var settings = dataAdapter.GetSettings();
            var watchedApplications = watchedApplicationDataAdapter.GetWatchedApplications();
            settings.WatchedApplicationSettings.WatchedApplications = new ObservableCollection<WatchedApplicationViewModel>(watchedApplications);
            var availableLanguages = languageDataAdapter.GetAllApplicationLanguages();
            var currentLanguage =
                languageDataAdapter.GetApplicationLanguageByCode(settings.LanguageSettings.CurrentLanguage.Code);
            var applicationLanguageViewModels = availableLanguages.ToList();
            settings.LanguageSettings.AvailableLanguages = new ObservableCollection<ApplicationLanguageViewModel>(applicationLanguageViewModels);
            settings.LanguageSettings.CurrentLanguage = applicationLanguageViewModels?.Where(l => l.Code == currentLanguage.Code).FirstOrDefault();
            
            // TODO REMOVEME
            var fileToolbarInfo = new ToolbarInfoViewModel(null)
            {
                BandOccupied = 0, IsVisible = true, ToolbarName = "FileToolbar", ToolbarFriendlyName = "File",
                OrdinalPositionInBand = 0
            };
            var editToolbarInfo = new ToolbarInfoViewModel(null)
            {
                BandOccupied = 1, IsVisible = true, ToolbarName = "EditToolbar", ToolbarFriendlyName = "Modifica",
                OrdinalPositionInBand = 0
            };
            settings.ToolbarInfo.Add(fileToolbarInfo);
            settings.ToolbarInfo.Add(editToolbarInfo);
            //TODO
            
            return settings;
        }
    }
}
