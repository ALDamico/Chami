using System.Data;
using Chami.Db.Annotations;
using Chami.Db.Entities;
using FluentMigrator;

namespace ChamiDbMigrations.Migrations
{
    [Migration(202301270001)]
    public class Categories : Migration
    {
        private readonly string _categoryName = AnnotationUtils.GetTableName(typeof(Category));
        private readonly string _environmentsTableName = AnnotationUtils.GetTableName(typeof(Environment));
        public override void Up()
        {
            Create.Table(_categoryName)
                .WithColumn(nameof(Category.Id)).AsInt32().PrimaryKey()
                .WithColumn(nameof(Category.Name)).AsString().NotNullable()
                .WithColumn(nameof(Category.BackgroundColor)).AsString().Nullable()
                .WithColumn(nameof(Category.Icon)).AsString().Nullable()
                .WithColumn(nameof(Category.Visibility)).AsBoolean().Nullable();

            Alter.Table(_environmentsTableName).AddColumn(nameof(Environment.CategoryId)).AsInt32()
                .ForeignKey(_categoryName, nameof(Category.Id)).OnDelete(Rule.SetNull).Nullable();
        }

        public override void Down()
        {
            Delete.Column(nameof(Environment.CategoryId)).FromTable(_environmentsTableName);
            Delete.Table(_categoryName);
        }
    }
}