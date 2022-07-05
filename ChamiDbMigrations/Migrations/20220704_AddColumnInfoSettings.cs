using System.Collections;
using System.Collections.Generic;
using Chami.Db.Annotations;
using Chami.Db.Entities;
using FluentMigrator;
using Newtonsoft.Json;

namespace ChamiDbMigrations.Migrations
{
    [Migration(20220704)]
    public class AddColumnInfoSettings:Migration 
    {
        public AddColumnInfoSettings()
        {
            _tableName = AnnotationUtils.GetTableName(typeof(Setting));
            _columnInfoTableName = AnnotationUtils.GetTableName(typeof(ColumnInfo));
            _columnInfoSetting = new Setting()
            {
                SettingName = "ColumnInfos",
                ViewModelName = "ChamiUI.PresentationLayer.ViewModels.HealthCheckSettingsViewModel",
                PropertyName = "HealthCheckSettings",
                Type = "System.Collections.IList",
                AssemblyName = "System.Collections",
                Value = ""
            };
        }
        public override void Up()
        {
            Create.Table(_columnInfoTableName)
                .WithColumn(nameof(ColumnInfo.Id)).AsInt64().PrimaryKey().Identity()
                .WithColumn(nameof(ColumnInfo.SettingName)).AsString().NotNullable()
                .ForeignKey(_tableName, nameof(Setting.SettingName))
                .WithColumn(nameof(ColumnInfo.IsVisible)).AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn(nameof(ColumnInfo.ColumnWidth)).AsDouble().NotNullable().WithDefaultValue(350d)
                .WithColumn(nameof(ColumnInfo.Binding)).AsString().Nullable()
                .WithColumn(nameof(ColumnInfo.OrdinalPosition)).AsInt64().Nullable()
                .WithColumn(nameof(ColumnInfo.Header)).AsString().Nullable()
                .WithColumn(nameof(ColumnInfo.Converter)).AsString().Nullable()
                .WithColumn(nameof(ColumnInfo.ConverterParameter)).AsString().Nullable();
            var columnInfos = GetColumnInfos();

            foreach (var columnInfo in columnInfos)
            {
                Insert.IntoTable(_columnInfoTableName).Row(columnInfo);
            }

            Insert.IntoTable(_tableName).Row(_columnInfoSetting);
        }

        public override void Down()
        {
            Delete.Table(_columnInfoTableName);
            Delete.FromTable(_tableName).Row(_columnInfoSetting);
        }

        private readonly string _tableName;
        private readonly string _columnInfoTableName;
        private readonly Setting _columnInfoSetting;

        private List<ColumnInfo> GetColumnInfos()
        {
            int i = 1;
            var list = new List<ColumnInfo>();
            var environmentVariableNameColumn = new ColumnInfo()
            {
                Id = i,
                IsVisible = true,
                ColumnWidth = 450d,
                Binding = "EnvironmentVariable.name",
                OrdinalPosition = 0,
                Header = "EnvironmentVariableNameHealthWindowColumn",
                SettingName = "ColumnInfos"
            };
            list.Add(environmentVariableNameColumn);
            i++;
            var expectedValueColumn = new ColumnInfo()
            {
                Id = i,
                IsVisible = true,
                ColumnWidth = 450d,
                Binding = "ExpectedValue",
                OrdinalPosition = 1,
                Header = "ExpectedValueHealthWindowColumn",
                SettingName = "ColumnInfos"
            };
            list.Add(expectedValueColumn);
            i++;
            var actualValueColumn = new ColumnInfo()
            {
                Id = i,
                IsVisible = true,
                ColumnWidth = 450d,
                Binding = "ActualValue",
                OrdinalPosition = 2,
                Header = "EnvironmentVariableValueHealthWindowColumn",
                SettingName = "ColumnInfos"
            };
            list.Add(actualValueColumn);
            i++;
            var statusColumn = new ColumnInfo()
            {
                Id = i,
                IsVisible = true,
                Binding = "IssueType",
                ColumnWidth = 200,
                OrdinalPosition = 3,
                Header = "EnvironmentVariableStatusHealthWindowColumn",
                Converter = "ChamiUI.PresentationLayer.Converters.EnvironmentHealthTypeConverter",
                ConverterParameter = "ShortDescription",
                SettingName = "ColumnInfos"
            };
            list.Add(statusColumn);
            return list;
        }
    }
}