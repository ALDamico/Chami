using Chami.Db.Annotations;
using Chami.Db.Entities;
using FluentMigrator;

namespace ChamiDbMigrations.Migrations
{
    [Migration(2023030100001)]
    public class Tags : Migration 
    {
        public override void Up()
        {
            var tagTableName = AnnotationUtils.GetTableName(typeof(Tag));

            Create.Table(tagTableName)
                .WithColumn(nameof(Tag.Id)).AsInt32().PrimaryKey().NotNullable()
                .WithColumn(nameof(Tag.Code)).AsString().NotNullable()
                .WithColumn(nameof(Tag.Description)).AsString().Nullable()
                .WithColumn(nameof(Tag.IsUserDefined)).AsBoolean().NotNullable().WithDefaultValue(true);

            var tagAssociationTableName = AnnotationUtils.GetTableName(typeof(EnvironmentTagAssociation));
            var environmentsTableName = AnnotationUtils.GetTableName(typeof(Environment));

            Create.Table(tagAssociationTableName)
                .WithColumn(nameof(EnvironmentTagAssociation.Id)).AsInt32().PrimaryKey().NotNullable()
                .WithColumn(nameof(EnvironmentTagAssociation.EnvironmentId)).AsInt32().NotNullable()
                .ForeignKey(environmentsTableName, nameof(Environment.EnvironmentId))
                .WithColumn(nameof(EnvironmentTagAssociation.TagId)).AsInt32().NotNullable()
                .ForeignKey(tagTableName, nameof(Tag.Id))
                .WithColumn(nameof(EnvironmentTagAssociation.IsUserDefined)).AsBoolean().NotNullable()
                .WithDefaultValue(true);
            
            throw new System.NotImplementedException();
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}