using System.Collections.Generic;
using System.Linq;
using Chami.Db.Entities;
using Dapper;

namespace Chami.Db.Repositories
{
    public class SettingsRepository : RepositoryBase
    {
        public SettingsRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public IEnumerable<Setting> GetSettings()
        {
            var queryString = @"
                SELECT SettingName, ViewModelName, Type, Value, PropertyName, AssemblyName, Converter
                FROM Settings;
";
            using var connection = GetConnection();
            return connection.Query<Setting>(queryString);
        }

        public Setting UpdateSetting(string settingName, string settingValue)
        {
            var queryString = @"
                UPDATE Settings
                SET Value = ?
                WHERE SettingName = ?
";
            using var connection = GetConnection();
            connection.Execute(queryString, new { settingValue, settingName });
            var setting = GetSettings().FirstOrDefault(s => s.SettingName == settingName);
            return setting;
        }

        public Setting UpdateSetting(Setting setting)
        {
            var queryString = @"
                UPDATE Settings
                SET Value = ?
                WHERE SettingName = ?
";
            using var connection = GetConnection();
            connection.Execute(queryString, new { setting.Value, setting.SettingName });
            return setting;
        }

    }
}