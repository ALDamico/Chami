using Chami.Db.Annotations;
using Chami.Db.Entities;
using FluentMigrator;

namespace ChamiDbMigrations.Migrations
{
   // [Migration(20220419)]
    public class AdvancedExportSettings : Migration
    {
        public override void Up()
        {
            Insert.IntoTable(AnnotationUtils.GetTableName(typeof(Setting))).Row(_maxLengthSetting);
        }

        public override void Down()
        {
            Delete.FromTable(AnnotationUtils.GetTableName(typeof(Setting))).Row(_maxLengthSetting);
        }

        private static readonly Setting _maxLengthSetting = new Setting()
        {
            
        };
    }
}