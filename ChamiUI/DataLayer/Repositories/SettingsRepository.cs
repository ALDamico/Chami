using System.Collections;
using System.Collections.Generic;
using ChamiUI.DataLayer.Entities;
using Dapper;

namespace ChamiUI.DataLayer.Repositories
{
    public class SettingsRepository : RepositoryBase
    {
        public SettingsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Setting> GetSettings()
        {
            var queryString = @"
                SELECT SettingName, ViewModelName, Type, Value
                FROM Settings;
";
            using var connection = GetConnection();
            return connection.Query<Setting>(queryString);
        }

        public Setting UpdateSetting(Setting setting)
        {
            var queryString = @"
                UPDATE Settings
                SET Value = ?
                WHERE SettingName = ?
";
            using var connection = GetConnection();
            connection.Execute(queryString, new {setting.Value, setting.SettingName});
            return setting;
        }

        private string _connectionString;
    }
}