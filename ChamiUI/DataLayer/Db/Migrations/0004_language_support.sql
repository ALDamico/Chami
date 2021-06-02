CREATE TABLE UiLanguages
(
    Code     TEXT PRIMARY KEY,
    Name     TEXT,
    IconPath TEXT
);


INSERT INTO Settings (SettingName, ViewModelName, "Type", Value, PropertyName, AssemblyName, Converter)
VALUES ('ApplicationLanguage', 'ChamiUI.PresentationLayer.ViewModels.LanguageSelectorViewModel',
        'System.Globalization.CultureInfo', 'en-US', 'LanguageSettings', NULL, NULL);
