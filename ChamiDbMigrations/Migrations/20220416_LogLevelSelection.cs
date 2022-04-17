using Chami.Db.Annotations;
using Chami.Db.Entities;
using FluentMigrator;

namespace ChamiDbMigrations.Migrations
{
    [Migration(20220416)]
    public class LogLevelSelection : Migration
    {
        public override void Up()
        {
            Insert.IntoTable(AnnotationUtils.GetTableName(typeof(Setting))).Row(_minimumLogLevelSetting);
        }

        public override void Down()
        {
            Delete.FromTable(AnnotationUtils.GetTableName(typeof(Setting))).Row(_minimumLogLevelSetting);
        }

        private static Setting _minimumLogLevelSetting = new Setting()
        {
            SettingName = "MinimumLogLevel",
            ViewModelName = "ChamiUI.PresentationLayer.ViewModels.LoggingSettingsViewModel",
            Type = "Serilog.Events.LogEventLevel",
            Value = "Information",
            PropertyName = "LoggingSettings",
            AssemblyName = "Serilog"
        };
    }
}