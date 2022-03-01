using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.PresentationLayer.Factories
{
    public static class SettingsCategoriesFactory
    {
        public static LanguageSelectorViewModel GetLanguageSettingCategory(SettingsViewModel settings)
        {
            var languageSettings = settings.LanguageSettings;
            languageSettings.DisplayName = ChamiUIStrings.LanguageCategory;
            languageSettings.Description = ChamiUIStrings.LanguageCategoryDescription;
            return languageSettings;
        }
    }
}