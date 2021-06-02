CREATE TABLE UiLanguages
(
    Code     TEXT PRIMARY KEY,
    Name     TEXT,
    IconPath TEXT
);


INSERT INTO Settings (SettingName, ViewModelName, "Type", Value, PropertyName, AssemblyName, Converter)
VALUES ('CurrentLanguage', 'ChamiUI.PresentationLayer.ViewModels.LanguageSelectorViewModel',
        'ChamiUI.PresentationLayer.ViewModels.ApplicationLanguageViewModel', 'en-US', 'LanguageSettings', NULL, NULL);
