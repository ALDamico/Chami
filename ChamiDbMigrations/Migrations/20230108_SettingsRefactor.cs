using Chami.Db.Annotations;
using Chami.Db.Entities;
using Dapper;
using FluentMigrator;

namespace ChamiDbMigrations.Migrations
{
    [Migration(202301080001)]
    public class SettingsRefactor : Migration
    {
        public override void Up()
        {
            var settingsBackupTableName = AnnotationUtils.GetTableName(typeof(Setting)) + "_bak";

            Create.Table(settingsBackupTableName).WithColumn("SettingName").AsString()
                .WithColumn("ViewModelName").AsString()
                .WithColumn("Type").AsString()
                .WithColumn("Value").AsString()
                .WithColumn("PropertyName").AsString().Nullable()
                .WithColumn("AssemblyName").AsString().Nullable()
                .WithColumn("Converter").AsString().Nullable();
            
            Execute.WithConnection(((connection, transaction) =>
            {
                connection.Execute($"INSERT INTO {settingsBackupTableName} SELECT * FROM Settings");
            }));
            
            Delete.Table(AnnotationUtils.GetTableName(typeof(Setting)));
            
            Create.Table("Settings")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("SettingName").AsString()
                .WithColumn("ViewModelName").AsString()
                .WithColumn("Type").AsString()
                .WithColumn("Value").AsString()
                .WithColumn("PropertyName").AsString().Nullable()
                .WithColumn("AssemblyName").AsString().Nullable()
                .WithColumn("Converter").AsString().Nullable();

            
            Execute.WithConnection(((connection, transaction) =>
            {

                connection.Execute($"INSERT INTO Settings(SettingName, ViewModelName, Type, Value, PropertyName, AssemblyName, Converter) SELECT SettingName, ViewModelName, Type, Value, PropertyName, AssemblyName, Converter FROM {settingsBackupTableName} ");
            }));

            Delete.Table(settingsBackupTableName);
        }

        public override void Down()
        {
            var settingsBackupTableName = AnnotationUtils.GetTableName(typeof(Setting)) + "_bak";

            Create.Table(settingsBackupTableName).WithColumn("SettingName").AsString()
                .WithColumn("ViewModelName").AsString()
                .WithColumn("Type").AsString()
                .WithColumn("Value").AsString()
                .WithColumn("PropertyName").AsString().Nullable()
                .WithColumn("AssemblyName").AsString().Nullable()
                .WithColumn("Converter").AsString().Nullable();
            
            Execute.WithConnection(((connection, transaction) =>
            {
                
                connection.Execute($"INSERT INTO {settingsBackupTableName} SELECT * FROM Settings");
            }));
            
            Delete.Table(AnnotationUtils.GetTableName(typeof(Setting)));
            
            Create.Table("Settings")
                .WithColumn("SettingName").AsString().PrimaryKey()
                .WithColumn("ViewModelName").AsString()
                .WithColumn("Type").AsString()
                .WithColumn("Value").AsString()
                .WithColumn("PropertyName").AsString().Nullable()
                .WithColumn("AssemblyName").AsString().Nullable()
                .WithColumn("Converter").AsString().Nullable();
            
            Execute.WithConnection(((connection, transaction) =>
            {
                connection.Execute($"INSERT INTO Settings(SettingName, ViewModelName, Type, Value, PropertyName, AssemblyName, Converter) SELECT SettingName, ViewModelName, Type, Value, PropertyName, AssemblyName, Converter FROM {settingsBackupTableName} ");
            }));

            Delete.Table(settingsBackupTableName);
        }
    }
}