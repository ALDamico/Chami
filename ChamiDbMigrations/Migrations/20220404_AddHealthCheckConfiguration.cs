using System;
using FluentMigrator;
using Chami.Db.Entities;

namespace ChamiDbMigrations.Migrations
{
    [Migration(202204041822)]
    public class AddHealthCheckConfiguration : Migration 
    {
        public override void Up()
        {
            Insert.IntoTable("Settings").Row(_healthCheckEnabledSetting);
            Insert.IntoTable("Settings").Row(_healthCheckTimeoutSetting);
        }

        public override void Down()
        {
            Delete.FromTable("Settings").Row(_healthCheckEnabledSetting);
            Delete.FromTable("Settings").Row(_healthCheckTimeoutSetting);
        }

        private static readonly Setting _healthCheckEnabledSetting = new Setting()
        {
            SettingName = "IsEnabled",
            ViewModelName = "ChamiUI.PresentationLayer.ViewModels.HealthCheckSettingsViewModel",
            PropertyName = "HealthCheckSettings",
            Type = "bool",
            Value = "False"
        };
        
        private static readonly Setting _healthCheckTimeoutSetting = new Setting()
        {
            SettingName = "TimeToCheck",
            ViewModelName = "ChamiUI.PresentationLayer.ViewModels.HealthCheckSettingsViewModel",
            PropertyName = "HealthCheckSettings",
            Type = "System.TimeSpan",
            Value = "60000",
            Converter = "ChamiUI.BusinessLayer.Converters.TimeSpanConverter",
            AssemblyName = "System.Private.CoreLib"
        };
    }
}