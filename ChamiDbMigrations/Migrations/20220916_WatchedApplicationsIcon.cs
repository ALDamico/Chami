using Chami.Db.Annotations;
using Chami.Db.Entities;
using FluentMigrator;

namespace ChamiDbMigrations.Migrations
{
    [Migration(2022091600001)]
    public class WatchedApplicationsIcon : Migration 
    {
        private static string _tableName =AnnotationUtils.GetTableName(typeof(WatchedApplication));
        private static string _applicationIconColumnName = nameof(WatchedApplication.ApplicationIcon);
        private static string _showInRunApplicationMenuColumnName = nameof(WatchedApplication.ShowInRunApplicationMenu);
        private static string _applicationPath = nameof(WatchedApplication.Path);
        public override void Up()
        {
            Alter.Table(_tableName).AddColumn(_applicationIconColumnName).AsBinary().Nullable();
            Alter.Table(_tableName).AddColumn(_showInRunApplicationMenuColumnName).AsBoolean().NotNullable()
                .WithDefaultValue(true).SetExistingRowsTo(true);
            Alter.Table(_tableName).AddColumn(_applicationPath).AsString().Nullable();
        }

        public override void Down()
        {
            Delete.Column(_applicationIconColumnName).FromTable(_tableName);
            Delete.Column(_showInRunApplicationMenuColumnName).FromTable(_tableName);
            Delete.Column(_applicationPath).FromTable(_tableName);
        }
    }
}