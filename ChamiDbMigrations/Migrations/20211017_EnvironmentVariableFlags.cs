using Chami.Db.Annotations;
using Chami.Db.Entities;
using FluentMigrator;

namespace ChamiDbMigrations.Migrations
{
    [Migration(202110171827)]
    public class EnvironmentVariableFlags : Migration 
    {
        public override void Up()
        {
            Alter.Table(AnnotationUtils.GetTableName(typeof(EnvironmentVariable)))
                .AddColumn(nameof(EnvironmentVariable.IsFolder)).AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Column(nameof(EnvironmentVariable.IsFolder))
                .FromTable(AnnotationUtils.GetTableName(typeof(EnvironmentVariable)));
        }
    }
}