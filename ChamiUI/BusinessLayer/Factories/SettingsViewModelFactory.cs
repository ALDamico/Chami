using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.PresentationLayer.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;

namespace ChamiUI.BusinessLayer.Factories
{
    public static class SettingsViewModelFactory
    {
        public static SettingsViewModel GetSettings(SettingsDataAdapter dataAdapter, WatchedApplicationDataAdapter watchedApplicationDataAdapter, ApplicationLanguageDataAdapter languageDataAdapter)
        {
            var settings = dataAdapter.GetSettings();
            var watchedApplications = watchedApplicationDataAdapter.GetActiveWatchedApplications();
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
