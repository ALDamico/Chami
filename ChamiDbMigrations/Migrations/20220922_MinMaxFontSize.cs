using Chami.Db.Annotations;
using Chami.Db.Entities;
using FluentMigrator;

namespace ChamiDbMigrations.Migrations
{
    [Migration(202209220001)]
    public class MinMaxFontSize : Migration
    {
        private readonly string _tableName = AnnotationUtils.GetTableName(typeof(Setting));

        private readonly Setting _minFontSizeSetting = new Setting()
        {
            SettingName = "MinFontSize",
            ViewModelName = "ChamiUI.PresentationLayer.ViewModels.ConsoleAppearanceViewModel",
            Type = "Nullable<double>",
            Value = "10",
            PropertyName = "ConsoleAppearanceSettings"

        };
        private readonly Setting _maxFontSizeSetting = new Setting()
        {
            SettingName = "MaxFontSize",
            ViewModelName = "ChamiUI.PresentationLayer.ViewModels.ConsoleAppearanceViewModel",
            Type = "Nullable<double>",
            Value = "null",
            PropertyName = "ConsoleAppearanceSettings"

        };

        private readonly Setting _saveFontSizeOnExitSetting = new Setting()
        {
            SettingName = "SaveFontSizeOnApplicationExit",
            ViewModelName = "ChamiUI.PresentationLayer.ViewModels.ConsoleAppearanceViewModel",
            Type = "bool",
            Value = "true",
            PropertyName = "ConsoleAppearanceSettings"
        };
        
        private readonly Setting _enableFontSizeResizingWithScrollWheelSetting = new Setting()
        {
            SettingName = "EnableFontSizeResizingWithScrollWheel",
            ViewModelName = "ChamiUI.PresentationLayer.ViewModels.ConsoleAppearanceViewModel",
            Type = "bool",
            Value = "true",
            PropertyName = "ConsoleAppearanceSettings"
        };
        
        public override void Up()
        {
            Insert.IntoTable(_tableName)
                .Row(_minFontSizeSetting)
                .Row(_maxFontSizeSetting)
                .Row(_saveFontSizeOnExitSetting)
                .Row(_enableFontSizeResizingWithScrollWheelSetting);
        }

        public override void Down()
        {
            Delete.FromTable(_tableName)
                .Row(_minFontSizeSetting)
                .Row(_maxFontSizeSetting)
                .Row(_saveFontSizeOnExitSetting)
                .Row(_enableFontSizeResizingWithScrollWheelSetting);
        }
    }
}