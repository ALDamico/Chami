using System.IO;
using Chami.Db.Entities;
using FluentMigrator;

namespace ChamiDbMigrations.Migrations
{
    [Migration(202106240002)]
    public class AddDetectorSettings : Migration {
        static AddDetectorSettings()
        {
            _isDetectionEnabledSetting = new Setting()
            {
                SettingName = "IsDetectionEnabled",
                ViewModelName = "ChamiUI.PresentationLayer.ViewModels.WatchedApplicationControlViewModel",
                Type = "bool",
                Value = "true",
                PropertyName = "WatchedApplicationSettings"
            };
        }

        private static readonly Setting _isDetectionEnabledSetting;
        public override void Up()
        {
            var isDetectionEnabledSetting = 
            Insert.IntoTable("Settings").Row(_isDetectionEnabledSetting);
            Create.Table("WatchedApplications")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("IsWatchEnabled").AsInt64();
        }

        public override void Down()
        {
            Delete.FromTable("Settings").Row(_isDetectionEnabledSetting);
            Delete.Table("WatchedApplications");
        }
    }
}