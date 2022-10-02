using Chami.Db.Annotations;
using Chami.Db.Entities;
using FluentMigrator;

namespace ChamiDbMigrations.Migrations
{
    [Migration(202209252015)]
    public class FontSizeChangeStep : Migration
    {
        private readonly string _tableName = AnnotationUtils.GetTableName(typeof(Setting));

        private readonly Setting _fontSizeStepChangeSetting = new Setting()
        {
            SettingName = "FontSizeStepChange",
            ViewModelName = "ChamiUI.PresentationLayer.ViewModels.ConsoleAppearanceViewModel",
            Type = "double",
            Value = "1",
            PropertyName = "ConsoleAppearanceSettings"

        };
        public override void Up()
        {
            Insert.IntoTable(_tableName).Row(_fontSizeStepChangeSetting);
        }

        public override void Down()
        {
            Delete.FromTable(_tableName).Row(_fontSizeStepChangeSetting);
        }
    }
}