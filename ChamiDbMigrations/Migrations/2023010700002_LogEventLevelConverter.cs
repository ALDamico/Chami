using Chami.Db.Annotations;
using Chami.Db.Entities;
using Chami.Db.Utils;
using FluentMigrator;

namespace ChamiDbMigrations.Migrations
{
    [Migration(2023010700002)]
    public class LogEventLevelConverter : Migration 
    {
        public LogEventLevelConverter()
        {
            _settingToUpdate = new {Converter = "ChamiUI.BusinessLayer.Converters.LogEventLevelConverter"};
            _whereClause = new {SettingName = "MinimumLogLevel"};
        }

        private readonly object _settingToUpdate;
        private readonly object _whereClause;
        
        public override void Up()
        {
            Update.Table(AnnotationUtils.GetTableName<Setting>()).Set(_settingToUpdate).Where(_whereClause);
        }

        public override void Down()
        {
            Update.Table((AnnotationUtils.GetTableName<Setting>())).Set(null).Where(_whereClause);
        }
    }
}