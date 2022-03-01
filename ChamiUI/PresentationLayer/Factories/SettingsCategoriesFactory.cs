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

        public static SafeVariableViewModel GetSafeVariableSettingCategory(SettingsViewModel settings)
        {
            var safeVariableSettings = settings.SafeVariableSettings;
            safeVariableSettings.DisplayName = ChamiUIStrings.SafetyCategory;
            safeVariableSettings.Description = ChamiUIStrings.SafetyCategoryDescription;
            return safeVariableSettings;
        }

        public static WatchedApplicationControlViewModel GetWatchedApplicationsSettingCategory(
            SettingsViewModel settings)
        {
            var watchedApplicationViewModel = settings.WatchedApplicationSettings;
            watchedApplicationViewModel.DisplayName = ChamiUIStrings.DetectorCategory;
            watchedApplicationViewModel.Description = ChamiUIStrings.DetectorCategoryDescription;
            return watchedApplicationViewModel;
        }

        public static MinimizationBehaviourViewModel GetMinimizationBehaviourSettingCategory(SettingsViewModel settings)
        {
            var minimizationBehaviour = settings.MinimizationBehaviour;
            minimizationBehaviour.DisplayName = ChamiUIStrings.MinimizationCategory;
            minimizationBehaviour.Description = ChamiUIStrings.MinimizationCategoryDescription;
            return minimizationBehaviour;
        }
    }
}