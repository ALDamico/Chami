using ChamiDbMigrations.Entities;
using FluentMigrator;

namespace ChamiDbMigrations.Migrations
{
    [Migration(202106240003)]
    public class AddLanguageSupport : Migration {
        static AddLanguageSupport()
        {
            _currentLanguageSetting = new Setting()
            {
                SettingName = "CurrentLanguage",
                ViewModelName = "ChamiUI.PresentationLayer.ViewModels.LanguageSelectorViewModel",
                Type = "ChamiUI.PresentationLayer.ViewModels.ApplicationLanguageViewModel",
                Value = "en-US",
                PropertyName = "LanguageSettings",
                AssemblyName = "ChamiUI",
                Converter = "ChamiUI.BusinessLayer.Converters.ApplicationLanguageSettingConverter"
            };

            _englishLanguage = new UiLanguage()
            {
                Code = "en-US",
                Name = "English",
                FlagPath = "/ChamiUI;component/Assets/Flags/us.svg"
            };

            _italianLanguage = new UiLanguage()
            {
                Code = "en-IT",
                Name = "Italiano",
                FlagPath = "/ChamiUI;component/Assets/Flags/it.svg"
            };
        }

        private static readonly Setting _currentLanguageSetting;
        private static readonly UiLanguage _englishLanguage;
        private static readonly UiLanguage _italianLanguage;
        public override void Up()
        {
            Insert.IntoTable("Settings").Row(_currentLanguageSetting);
            Create.Table("UiLanguages")
                .WithColumn("Code").AsString().PrimaryKey()
                .WithColumn("Name").AsString()
                .WithColumn("FlagPath").AsString();
            Insert.IntoTable("UiLanguages").Row(_englishLanguage);
            Insert.IntoTable("UiLanguages").Row(_italianLanguage);

        }

        public override void Down()
        {
            Delete.FromTable("Settings").Row(_currentLanguageSetting);
            Delete.Table("UiLanguages");
        }
    }
}