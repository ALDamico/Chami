using System.Collections.Generic;
using System.Linq;
using Chami.Db.Entities;
using Dapper;

namespace Chami.Db.Repositories
{
    /// <summary>
    /// Performs CRUD operations on the Settings aggregate.
    /// </summary>
    public class SettingsRepository : RepositoryBase
    {
        /// <summary>
        /// Constructs a new <see cref="SettingsRepository"/> object with the provided connection string.
        /// </summary>
        /// <param name="connectionString">The connection string for the database to connect to.</param>
        public SettingsRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Retrieves all the <see cref="Setting"/>s object associated with the Chami application.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> containing all the <see cref="Setting"/>s objects.</returns>
        public IEnumerable<Setting> GetSettings()
        {
            var queryString = @"
                SELECT SettingName, ViewModelName, Type, Value, PropertyName, AssemblyName, Converter
                FROM Settings;
";
            using var connection = GetConnection();
            return connection.Query<Setting>(queryString);
        }

        /// <summary>
        /// Updates the value of a <see cref="Setting"/>.
        /// </summary>
        /// <param name="settingName">The name of the <see cref="Setting"/> to update.</param>
        /// <param name="settingValue">The new value for the <see cref="Setting"/> to update.</param>
        /// <returns>Returns the setting with the requested name if it exists, otherwise null.</returns>
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

        /// <summary>
        /// Updates the value of a <see cref="Setting"/>.
        /// </summary>
        /// <param name="setting">The <see cref="Setting"/> to update.</param>
        /// <returns>Returns the setting with the requested name if it exists, otherwise null.</returns>
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