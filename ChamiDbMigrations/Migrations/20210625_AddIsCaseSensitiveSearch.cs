using Chami.Db.Entities;
using FluentMigrator;

namespace ChamiDbMigrations.Migrations
{
    [Migration(20210625)]
    public class AddIsCaseSensitiveSearch : Migration {
        static AddIsCaseSensitiveSearch()
        {
            _isCaseSensitiveSearch = new Setting()
            {
                SettingName = "IsCaseSensitiveSearch",
                ViewModelName = "ChamiUI.PresentationLayer.ViewModels.MainWindowSavedBehaviourViewModel",
                PropertyName = "MainWindowBehaviourSettings",
                Type = "bool",
                Value = "false"
            };
        }

        private static readonly  Setting _isCaseSensitiveSearch;
        public override void Up()
        {
            Insert.IntoTable("Settings").Row(_isCaseSensitiveSearch);
        }

        public override void Down()
        {
            Delete.FromTable("Settings").Row(_isCaseSensitiveSearch);
        }
    }
}