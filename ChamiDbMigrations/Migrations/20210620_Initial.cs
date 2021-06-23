using System;
using System.Data;
using ChamiDbMigrations.Entities;
using FluentMigrator;

namespace ChamiDbMigrations.Migrations
{
    public class Initial :Migration
    {
        public override void Up()
        {
            Create.Table("EnvironmentTypes")
                .WithColumn("EnvironmentTypeId").AsInt64().PrimaryKey().Identity()
                .WithColumn("Description").AsString().NotNullable();

            Insert.IntoTable("EnvironmentTypes")
                .Row(new {EnvironmentTypeId = 0, Description = "Normal environment"})
                .Row(new {EnvironmentTypeId = 1, Description = "Backup environment"})
                .Row(new {EnvironmentTypeId = 2, Description = "Template environment"});
                
            Create.Table("Environments")
                .WithColumn("EnvironmentId").AsInt64().PrimaryKey().Identity()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("AddedOn").AsDateTime().WithDefaultValue(DateTime.Now)
                .WithColumn("EnvironmentType").AsInt64().WithDefaultValue(0)
                .ForeignKey("EnvironmentTypes", "EnvironmentTypeId");

            Create.Table("EnvironmentVariables")
                .WithColumn("EnvironmentVariableId").AsInt64().PrimaryKey().Identity()
                .WithColumn("Name").AsString().NotNullable().Unique()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("AddedOn").AsDateTime().WithDefaultValue(DateTime.Now)
                .WithColumn("EnvironmentId").AsInt64().ForeignKey("Environments", "EnvironmentId")
                    .OnDelete(Rule.Cascade).Indexed("ix_environment_variables_environment_id");

            Create.Table("Settings")
                .WithColumn("SettingName").AsString().PrimaryKey().Identity()
                .WithColumn("ViewModelName").AsString()
                .WithColumn("Type").AsString()
                .WithColumn("Value").AsString()
                .WithColumn("PropertyName").AsString()
                .WithColumn("AssemblyName").AsString()
                .WithColumn("Converter").AsString();

            

        }

        private void InsertSettings()
        {
            var loggingEnabledSetting = new Setting()
            {
                SettingName = "LoggingEnabled",
                ViewModelName = "ChamiUI.PresentationLayer.ViewModels.LoggingSettingsViewModel",
                Type = "bool",
                Value = "true",
                PropertyName = "LoggingSettings"
            };

            var enableSafeVarsSetting = new Setting()
            {
                SettingName = "EnableSafeVars",
                ViewModelName = "ChamiUI.PresentationLayer.ViewModels.SafeVariableViewModel",
                Type = "bool",
                Value = "true",
                PropertyName = "SafeVariableSettings"
            };

            var fontFamilySetting = new Setting()
            {
                SettingName = "FontFamily",
                ViewModelName = "ChamiUI.PresentationLayer.ViewModels.ConsoleAppearanceViewModel",
                Type = "System.Windows.Media.FontFamily",
                Value = "file:///c:/windows/fonts#Courier New",
                PropertyName = "ConsoleAppearanceSettings",
                AssemblyName = "PresentationCore"
            };

            var backgroundColorSetting = new Setting()
            {
                SettingName = "BackgroundColor",
                ViewModelName = "ChamiUI.PresentationLayer.ViewModels.ConsoleAppearanceViewModel",
                Type = "System.Windows.Media.SolidColorBrush",
                Value = "#FF000000",
                PropertyName = "ConsoleAppearanceSettings",
                AssemblyName = "PresentationCore",
                Converter = "ChamiUI.BusinessLayer.Converters.BrushConverter"
            };
            
            var foregroundColorSetting = new Setting()
            {
                SettingName = "ForegroundColor",
                ViewModelName = "ChamiUI.PresentationLayer.ViewModels.ConsoleAppearanceViewModel",
                Type = "System.Windows.Media.SolidColorBrush",
                Value = "#FF00FF00",
                PropertyName = "ConsoleAppearanceSettings",
                AssemblyName = "PresentationCore",
                Converter = "ChamiUI.BusinessLayer.Converters.BrushConverter"
            };

            var fontSizeSetting = new Setting()
            {

            };
            Insert.IntoTable("Settings")
                .Row(loggingEnabledSetting)
                .Row(enableSafeVarsSetting)
                .Row(fontFamilySetting)
                .Row(backgroundColorSetting)
                .Row(foregroundColorSetting)
                .Row(fontSizeSetting);
            
        }

        /*
         INSERT INTO Settings (SettingName,ViewModelName,"Type",Value,PropertyName,AssemblyName,Converter) VALUES
('BackgroundColor','ChamiUI.PresentationLayer.ViewModels.ConsoleAppearanceViewModel','System.Windows.Media.SolidColorBrush','#FF000000','ConsoleAppearanceSettings','PresentationCore','ChamiUI.BusinessLayer.Converters.BrushConverter'),
('ForegroundColor','ChamiUI.PresentationLayer.ViewModels.ConsoleAppearanceViewModel','System.Windows.Media.SolidColorBrush','#FF00FF00','ConsoleAppearanceSettings','PresentationCore','ChamiUI.BusinessLayer.Converters.BrushConverter'),
('FontSize','ChamiUI.PresentationLayer.ViewModels.ConsoleAppearanceViewModel','double','12','ConsoleAppearanceSettings',NULL,NULL); 
         */

        public override void Down()
        {
            Delete.Table("EnvironmentVariables");
            Delete.Table("EnvironmentTypes");
            Delete.Table("Environments");
            Delete.Table("Settings");
        }
    }
}