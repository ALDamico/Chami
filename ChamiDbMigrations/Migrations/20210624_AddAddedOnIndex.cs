using FluentMigrator;

namespace ChamiDbMigrations.Migrations
{
    [Migration(202106240001)]
    public class AddAddedOnIndex :Migration {
        public override void Up()
        {
            Create.Index("ix_date").OnTable("Environments").OnColumn("AddedOn");
        }

        public override void Down()
        {
            Delete.Index("ix_date").OnTable("Environments").OnColumn("AddedOn");
        }
    }
}