-- Environments definition

CREATE TABLE Environments (
                              EnvironmentId INTEGER PRIMARY KEY AUTOINCREMENT,
                              Name TEXT NOT NULL CHECK (Name <> ''),
                              AddedOn TEXT DEFAULT CURRENT_DATE
    , IsBackup INTEGER CHECK (IsBackup IN (0, 1)) DEFAULT 0 NOT NULL);

-- EnvironmentVariables definition

CREATE TABLE "EnvironmentVariables"
(
    EnvironmentVariableId INTEGER
        primary key autoincrement,
    Name TEXT not null,
    Value TEXT not null,
    AddedOn TEXT default current_timestamp,
    EnvironmentId INTEGER
        references Environments,
    check (Name <> '')
);

CREATE INDEX ix_environment_variables_environment_id
    on EnvironmentVariables (EnvironmentId);

-- Settings definition

CREATE TABLE Settings (
                          SettingName TEXT PRIMARY KEY,
                          ViewModelName TEXT,
                          "Type" TEXT,
                          Value TEXT,
                          PropertyName TEXT,
                          AssemblyName TEXT
    , Converter String);

INSERT INTO Settings (SettingName,ViewModelName,"Type",Value,PropertyName,AssemblyName,Converter) VALUES
('LoggingEnabled','ChamiUI.PresentationLayer.ViewModels.LoggingSettingsViewModel','bool','True','LoggingSettings',NULL,NULL),
('EnableSafeVars','ChamiUI.PresentationLayer.ViewModels.SafeVariableViewModel','bool','True','SafeVariableSettings',NULL,NULL),
('FontFamily','ChamiUI.PresentationLayer.ViewModels.ConsoleAppearanceViewModel','System.Windows.Media.FontFamily','file:///c:/windows/fonts#Courier New','ConsoleAppearanceSettings','PresentationCore',NULL),
('BackgroundColor','ChamiUI.PresentationLayer.ViewModels.ConsoleAppearanceViewModel','System.Windows.Media.SolidColorBrush','#FF000000','ConsoleAppearanceSettings','PresentationCore','ChamiUI.BusinessLayer.Converters.BrushConverter'),
('ForegroundColor','ChamiUI.PresentationLayer.ViewModels.ConsoleAppearanceViewModel','System.Windows.Media.SolidColorBrush','#FF00FF00','ConsoleAppearanceSettings','PresentationCore','ChamiUI.BusinessLayer.Converters.BrushConverter'),
('FontSize','ChamiUI.PresentationLayer.ViewModels.ConsoleAppearanceViewModel','double','12','ConsoleAppearanceSettings',NULL,NULL); 