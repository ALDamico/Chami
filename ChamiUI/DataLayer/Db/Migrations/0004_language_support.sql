CREATE TABLE UiLanguages
(
    Code     TEXT PRIMARY KEY,
    Name     TEXT,
    FlagPath TEXT
);

INSERT INTO UiLanguages(Code, Name, FlagPath)
VALUES ('en-US', 'English', 'Assets/us.svg'),
       ('it-IT', 'Italiano', 'Assets/it.svg');


INSERT INTO Settings (SettingName, ViewModelName, "Type", Value, PropertyName, AssemblyName, Converter)
VALUES ('CurrentLanguage', 'ChamiUI.PresentationLayer.ViewModels.LanguageSelectorViewModel',
        'ChamiUI.PresentationLayer.ViewModels.ApplicationLanguageViewModel', 'en-US', 'LanguageSettings', 'ChamiUI', 'ChamiUI.BusinessLayer.Converters.ApplicationLanguageSettingConverter');
