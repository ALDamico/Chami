using Chami.Db.Entities;
using FluentMigrator;

namespace ChamiDbMigrations.Migrations
{
    [Migration(202109160001)]
    public class SaveWindowState : Migration
    {
        private static Setting _windowStateSetting;

        static SaveWindowState()
        {
            _windowStateSetting = new Setting()
            {
                SettingName = "WindowState",
                ViewModelName = "ChamiUI.PresentationLayer.ViewModels.MainWindowSavedBehaviourViewModel",
                PropertyName = "MainWindowBehaviourSettings",
                Type = "int",
                Value = "0"
            };
        }
        public override void Up()
        {
            Insert.IntoTable("Settings").Row(_windowStateSetting);
        }

        public override void Down()
        {
            Delete.FromTable("Settings").Row(_windowStateSetting);
        }
    }
}