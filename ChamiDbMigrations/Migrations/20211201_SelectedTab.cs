using Chami.Db.Entities;
using FluentMigrator;

namespace ChamiDbMigrations.Migrations
{
    [Migration(20211201)]
    public class SelectedTab : Migration
    {
        static SelectedTab()
        {
            _selectedEnvironmentTypeTab = new Setting()
            {
                SettingName = "SelectedEnvironmentTypeTab",
                ViewModelName = "ChamiUI.PresentationLayer.ViewModels.MainWindowSavedBehaviourViewModel",
                PropertyName = "MainWindowBehaviourSettings",
                Type = "int",
                Value = "0"
            };
        }
        private static readonly Setting _selectedEnvironmentTypeTab;
        public override void Up()
        {
            Insert.IntoTable("Settings").Row(_selectedEnvironmentTypeTab);
        }

        public override void Down()
        {
            Delete.FromTable("Settings").Row(_selectedEnvironmentTypeTab);
        }
    }
}