using FluentMigrator;

namespace ChamiDbMigrations.Migrations
{
    [Migration(202201030001)]
    public class TemplateReference : Migration
    {
        public override void Up()
        {
            Alter.Table("Environments").AddColumn("TemplateId").AsInt64().Nullable();
            Create.ForeignKey("FK_EnvironmentTemplate").FromTable("Environments").ForeignColumn("TemplateId")
                .ToTable("Environments").PrimaryColumn("EnvironmentId");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_EnvironmentTemplate");
            Delete.Column("TemplateId").FromTable("Environments");
        }
    }
}