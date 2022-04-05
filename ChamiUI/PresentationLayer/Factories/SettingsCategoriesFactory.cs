using System;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;
using MahApps.Metro.IconPacks;

namespace ChamiUI.PresentationLayer.Factories
{
    public static class SettingsCategoriesFactory
    {
        public static LanguageSelectorViewModel GetLanguageSettingCategory(SettingsViewModel settings)
        {
            var languageSettings = settings.LanguageSettings;
            languageSettings.DisplayName = ChamiUIStrings.LanguageCategory;
            languageSettings.Description = ChamiUIStrings.LanguageCategoryDescription;
            languageSettings.IconPath = Enum.GetName(PackIconFontAwesomeKind.LanguageSolid);
            return languageSettings;
        }

        public static LoggingSettingsViewModel GetLoggingSettingCategory(SettingsViewModel settings)
        {
            var loggingSettings = settings.LoggingSettings;
            loggingSettings.DisplayName = ChamiUIStrings.LoggingCategory;
            loggingSettings.Description = ChamiUIStrings.LoggingCategoryDescription;
            loggingSettings.IconPath = Enum.GetName(PackIconFontAwesomeKind.PencilAltSolid);
            return loggingSettings;
        }

        public static ConsoleAppearanceViewModel GetConsoleAppearanceCategory(SettingsViewModel settings)
        {
            var consoleAppearanceSettings = settings.ConsoleAppearanceSettings;
            consoleAppearanceSettings.DisplayName = ChamiUIStrings.ViewCategory;
            consoleAppearanceSettings.Description = ChamiUIStrings.ViewCategoryDescription;
            consoleAppearanceSettings.IconPath = Enum.GetName(PackIconFontAwesomeKind.TerminalSolid);
            return consoleAppearanceSettings;
        }

        public static SafeVariableViewModel GetSafeVariableSettingCategory(SettingsViewModel settings)
        {
            var safeVariableSettings = settings.SafeVariableSettings;
            safeVariableSettings.DisplayName = ChamiUIStrings.SafetyCategory;
            safeVariableSettings.Description = ChamiUIStrings.SafetyCategoryDescription;
            safeVariableSettings.IconPath = Enum.GetName(PackIconFontAwesomeKind.UserLockSolid);
            return safeVariableSettings;
        }

        public static WatchedApplicationControlViewModel GetWatchedApplicationsSettingCategory(
            SettingsViewModel settings)
        {
            var watchedApplicationViewModel = settings.WatchedApplicationSettings;
            watchedApplicationViewModel.DisplayName = ChamiUIStrings.DetectorCategory;
            watchedApplicationViewModel.Description = ChamiUIStrings.DetectorCategoryDescription;
            watchedApplicationViewModel.IconPath = Enum.GetName(PackIconFontAwesomeKind.MicroscopeSolid);
            return watchedApplicationViewModel;
        }

        public static MinimizationBehaviourViewModel GetMinimizationBehaviourSettingCategory(SettingsViewModel settings)
        {
            var minimizationBehaviour = settings.MinimizationBehaviour;
            minimizationBehaviour.DisplayName = ChamiUIStrings.MinimizationCategory;
            minimizationBehaviour.Description = ChamiUIStrings.MinimizationCategoryDescription;
            minimizationBehaviour.IconPath = Enum.GetName(PackIconFontAwesomeKind.WindowMaximizeRegular);
            return minimizationBehaviour;
        }

        public static HealthCheckSettingsViewModel GetHealthCheckSettingCategory(SettingsViewModel settings)
        {
            var healthCheckSettings = settings.HealthCheckSettings;
            healthCheckSettings.DisplayName = ChamiUIStrings.HealthCheckCategory;
            healthCheckSettings.Description = ChamiUIStrings.HealthCheckCategoryDescription;
            healthCheckSettings.IconPath = Enum.GetName(PackIconFontAwesomeKind.UserNurseSolid);
            return healthCheckSettings;
        }
    }
}