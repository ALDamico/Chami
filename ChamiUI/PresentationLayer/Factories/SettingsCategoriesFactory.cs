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

        public static LoggingSettingsViewModel GetLoggingSettingCategory(SettingsViewModel settings)
        {
            var loggingSettings = settings.LoggingSettings;
            loggingSettings.DisplayName = ChamiUIStrings.LoggingCategory;
            loggingSettings.Description = ChamiUIStrings.LoggingCategoryDescription;
            return loggingSettings;
        }

        public static ConsoleAppearanceViewModel GetConsoleAppearanceCategory(SettingsViewModel settings)
        {
            var consoleAppearanceSettings = settings.ConsoleAppearanceSettings;
            consoleAppearanceSettings.DisplayName = ChamiUIStrings.ViewCategory;
            consoleAppearanceSettings.Description = ChamiUIStrings.ViewCategoryDescription;
            return consoleAppearanceSettings;
        }
    }
}