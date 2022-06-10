using Chami.Db.Annotations;
using Chami.Db.Entities;
using FluentMigrator;

namespace ChamiDbMigrations.Migrations
{
    [Migration(20220419)]
    public class AdvancedExportSettings : Migration
    {
        public override void Up()
        {
            Insert.IntoTable(AnnotationUtils.GetTableName(typeof(Setting)))
                .Row(_maxLengthSetting)
                .Row(_variableNameColumnWidth)
                .Row(_isMarkedColumnWidth)
                .Row(_previewBackground)
                .Row(_previewForeground)
                .Row(_previewFontStyle)
                .Row(_previewFontSize);
        }

        public override void Down()
        {
            Delete.FromTable(AnnotationUtils.GetTableName(typeof(Setting)))
                .Row(_maxLengthSetting)
                .Row(_variableNameColumnWidth)
                .Row(_isMarkedColumnWidth)
                .Row(_previewBackground)
                .Row(_previewForeground)
                .Row(_previewFontStyle)
                .Row(_previewFontSize);
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

        private static readonly Setting _previewForeground = new Setting()
        {
            SettingName = "PreviewForegroundColor",
            ViewModelName = "ChamiUI.PresentationLayer.ViewModels.AdvancedExporterSettingsViewModel",
            Type = "System.Windows.Media.SolidColorBrush",
            Value = "#FF00FF00",
            PropertyName = "AdvancedExporterSettings",
            AssemblyName = "PresentationCore",
            Converter = "ChamiUI.BusinessLayer.Converters.BrushConverter"
        };

        private static readonly Setting _previewBackground = new Setting()
        {
            SettingName = "PreviewBackgroundColor",
            ViewModelName = "ChamiUI.PresentationLayer.ViewModels.AdvancedExporterSettingsViewModel",
            Type = "System.Windows.Media.SolidColorBrush",
            Value = "#FF000000",
            PropertyName = "AdvancedExporterSettings",
            AssemblyName = "PresentationCore",
            Converter = "ChamiUI.BusinessLayer.Converters.BrushConverter"
        };

        private static readonly Setting _previewFontStyle = new Setting()
        {
            SettingName = "SelectedFont",
            ViewModelName = "ChamiUI.PresentationLayer.ViewModels.AdvancedExporterSettingsViewModel",
            Type = "System.Windows.Media.FontFamily",
            Value = "file:///c:/windows/fonts#Courier New",
            PropertyName = "AdvancedExporterSettings",
            AssemblyName = "PresentationCore"
        };
        
        private static readonly Setting _previewFontSize = new Setting()
        {
            SettingName = "PreviewFontSize",
            ViewModelName = "ChamiUI.PresentationLayer.ViewModels.AdvancedExporterSettingsViewModel",
            PropertyName = "AdvancedExporterSettings",
            Type = "double",
            Value = "14"
        };
    }
}