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
            settings.LanguageSettings.AvailableLanguages = new ObservableCollection<ApplicationLanguageViewModel>(availableLanguages);
            settings.LanguageSettings.CurrentLanguage = availableLanguages?.Where(l => l.Code == currentLanguage.Code).FirstOrDefault();
            return settings;
        }
    }
}
