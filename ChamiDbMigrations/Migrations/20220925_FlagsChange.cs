using Chami.Db.Annotations;
using Chami.Db.Entities;
using FluentMigrator;

namespace ChamiDbMigrations.Migrations
{
    [Migration(2022092500001)]
    public class FlagsChange : Migration
    {
        private readonly string _uiLanguagesTable = AnnotationUtils.GetTableName(typeof(UiLanguage));
        public override void Up()
        {
            Update.Table(_uiLanguagesTable).Set(new {FlagPath="/Assets/Flags/us.svg"}).Where(new {Code="en-US"});
            Update.Table(_uiLanguagesTable).Set(new {FlagPath="/Assets/Flags/it.svg"}).Where(new {Code="it-IT"});
        }

        public override void Down()
        {
            Update.Table(_uiLanguagesTable).Set(new {FlagPath="/ChamiUI;component/Assets/Flags/us.svg"}).Where(new {Code="en-US"});
            Update.Table(_uiLanguagesTable).Set(new {FlagPath="/ChamiUI;component/Assets/Flags/it.svg"}).Where(new {Code="it-IT"});
        }
    }
}