using Chami.Db.Annotations;
using Chami.Db.Entities;
using FluentMigrator;

namespace ChamiDbMigrations.Migrations
{
   // [Migration(20220419)]
    public class AdvancedExportSettings : Migration
    {
        public override void Up()
        {
            Insert.IntoTable(AnnotationUtils.GetTableName(typeof(Setting)))
                .Row(_maxLengthSetting)
                .Row(_variableNameColumnWidth)
                .Row(_isMarkedColumnWidth)
                ;
        }

        public override void Down()
        {
            Delete.FromTable(AnnotationUtils.GetTableName(typeof(Setting))).Row(_maxLengthSetting);
        }

        private static readonly Setting _maxLengthSetting = new Setting()
        {
            SettingName = "MaxLineLength",
            ViewModelName = "ChamiUI.PresentationLayer.ViewModels.AdvancedExporterSettingsViewModel",
            PropertyName = "AdvancedExporterSettings",
            Type = "int",
            Value = "80"
        };

        private static readonly Setting _variableNameColumnWidth = new Setting()
        {
            SettingName = "VariableNameColumnWidth",
            ViewModelName = "ChamiUI.PresentationLayer.ViewModels.AdvancedExporterSettingsViewModel",
            PropertyName = "AdvancedExporterSettings",
            Type = "double",
            Value = "250"
        };

        private static readonly Setting _isMarkedColumnWidth = new Setting()
        {
            SettingName = "IsMarkedColumnWidth",
            ViewModelName = "ChamiUI.PresentationLayer.ViewModels.AdvancedExporterSettingsViewModel",
            PropertyName = "AdvancedExporterSettings",
            Type = "double",
            Value = "250"
        };
    }
}