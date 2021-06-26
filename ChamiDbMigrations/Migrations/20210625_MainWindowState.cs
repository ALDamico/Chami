using Chami.Db.Entities;
using FluentMigrator;

namespace ChamiDbMigrations.Migrations
{
    [Migration(202106250001)]
    public class MainWindowState : Migration {
        static MainWindowState()
        {
            _mainWindowHeightSetting = new Setting()
            {
                SettingName = "Height",
                ViewModelName = "ChamiUI.PresentationLayer.ViewModels.MainWindowSavedBehaviourViewModel",
                PropertyName = "MainWindowBehaviourSettings",
                Type = "double",
                Value = "450"
            };
            _mainWindowWidthSetting = new Setting()
            {
                SettingName = "Width",
                ViewModelName = "ChamiUI.PresentationLayer.ViewModels.MainWindowSavedBehaviourViewModel",
                PropertyName = "MainWindowBehaviourSettings",
                Type = "double",
                Value = "600"
            };
            _mainWindowXPositionSetting = new Setting()
            {
                SettingName = "XPosition",
                ViewModelName = "ChamiUI.PresentationLayer.ViewModels.MainWindowSavedBehaviourViewModel",
                PropertyName = "MainWindowBehaviourSettings",
                Type = "double",
                Value = "100"
            };
            _mainWindowYPositionSetting = new Setting()
            {
                SettingName = "YPosition",
                ViewModelName = "ChamiUI.PresentationLayer.ViewModels.MainWindowSavedBehaviourViewModel",
                PropertyName = "MainWindowBehaviourSettings",
                Type = "double",
                Value = "100"
            };
            _mainWindowSearchPathSetting = new Setting()
            {
                SettingName = "SearchPath",
                ViewModelName = "ChamiUI.PresentationLayer.ViewModels.MainWindowSavedBehaviourViewModel",
                PropertyName = "MainWindowBehaviourSettings",
                Type = "ChamiUI.PresentationLayer.Filtering.IFilterStrategy",
                Value = "ChamiUI.PresentationLayer.Filtering.EnvironmentNameFilterStrategy",
                Converter = "ChamiUI.BusinessLayer.Converters.FilterStrategyConverter"
            };
            
            /*// TODO
            // Probably unnecessary (we save this in _mainWindowSortingSetting)
            _mainWindowIsDescendingSortingSetting = new Setting()
            {
                SettingName = "IsDescendingSorting",
                ViewModelName = "ChamiUI.PresentationLayer.ViewModels.MainWindowSavedBehaviourViewModel",
                PropertyName = "MainWindowBehaviourSettings",
                Type = "bool",
                Value = "false"
            };*/
            _mainWindowSortingSetting = new Setting()
            {
                SettingName = "SortDescription",
                ViewModelName = "ChamiUI.PresentationLayer.ViewModels.MainWindowSavedBehaviourViewModel",
                PropertyName = "MainWindowBehaviourSettings",
                Type = "System.ComponentModel.SortDescription",
                Value = "Name#Ascending",
                AssemblyName = "WindowsBase", Converter = "ChamiUI.BusinessLayer.Converters.SortDescriptionConverter"
            };
        }

        private static readonly Setting _mainWindowHeightSetting;
        private static readonly Setting _mainWindowWidthSetting;
        private static readonly Setting _mainWindowXPositionSetting;
        private static readonly Setting _mainWindowYPositionSetting;
        private static readonly Setting _mainWindowSearchPathSetting;
       // private static readonly Setting _mainWindowIsDescendingSortingSetting;
        private static readonly Setting _mainWindowSortingSetting;
        public override void Up()
        {
            Insert.IntoTable("Settings")
                .Row(_mainWindowHeightSetting)
                .Row(_mainWindowWidthSetting)
                .Row(_mainWindowXPositionSetting)
                .Row(_mainWindowYPositionSetting)
                .Row(_mainWindowSearchPathSetting)
               // .Row(_mainWindowIsDescendingSortingSetting)
                .Row(_mainWindowSortingSetting);
        }

        public override void Down()
        {
            Delete.FromTable("Settings")
                .Row(_mainWindowHeightSetting)
                .Row(_mainWindowWidthSetting)
                .Row(_mainWindowXPositionSetting)
                .Row(_mainWindowYPositionSetting)
                .Row(_mainWindowSearchPathSetting)
              //  .Row(_mainWindowIsDescendingSortingSetting)
                .Row(_mainWindowSortingSetting);
        }
    }
}