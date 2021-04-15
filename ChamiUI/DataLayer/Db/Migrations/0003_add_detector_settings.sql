﻿INSERT INTO Settings
(SettingName, ViewModelName, "Type", Value, PropertyName, AssemblyName, Converter)
VALUES('IsDetectionEnabled', 'ChamiUI.PresentationLayer.ViewModels.WatchedApplicationControlViewModel', 'bool', 'true', 'WatchedApplicationSettings', NULL, NULL);

CREATE TABLE WatchedApplications (
    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL CHECK(Name <> ''),
    IsWatchEnabled INTEGER NOT NULL CHECK(IsWatchEnabled IN (0, 1))
);