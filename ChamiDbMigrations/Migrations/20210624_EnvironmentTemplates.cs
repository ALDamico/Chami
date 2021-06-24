using FluentMigrator;
using FluentMigrator.Builders.Rename.Column;

namespace ChamiDbMigrations.Migrations
{
    [Migration(202106240004)]
    public class EnvironmentTemplates : Migration {
        public override void Up()
        {
            Rename.Column("IsBackup").OnTable("Environments").To("EnvironmentType");
        }

        public override void Down()
        {
            Rename.Column("EnvironmentType").OnTable("Environments").To("IsBackup");
        }
    }
}