using Chami.Db.Annotations;
using Chami.Db.Entities;
using FluentMigrator;

namespace ChamiDbMigrations.Migrations
{
    [Migration(202210100001)]
    public class ApplicationIconWidthAndHeight : Migration
    {
        public override void Up()
        {
            Alter.Table(AnnotationUtils.GetTableName(typeof(WatchedApplication)))
                .AddColumn(nameof(WatchedApplication.IconHeight)).AsInt32().Nullable()
                .AddColumn(nameof(WatchedApplication.IconWidth)).AsInt32().Nullable();
        }

        public override void Down()
        {
            Delete.Column(nameof(WatchedApplication.IconHeight))
                .Column(nameof(WatchedApplication.IconWidth))
                .FromTable(AnnotationUtils.GetTableName(typeof(WatchedApplication)));
        }
    }
}