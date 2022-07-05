using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<EnvironmentVariableBlacklist> UpsertBlacklistedVariableAsync(EnvironmentVariableBlacklist blacklistedVariable)
        {
            if (blacklistedVariable == null)
            {
                throw new InvalidOperationException("Attempting to persist null entity.");
            }

            if (blacklistedVariable.Id == 0)
            {
                return await InsertBlacklistedVariableAsync(blacklistedVariable);
            }

            return await UpdateBlacklistedVariableAsync(blacklistedVariable);
        }
        
        public async Task<EnvironmentVariableBlacklist> InsertBlacklistedVariableAsync(
            EnvironmentVariableBlacklist blackistedVariable)
        {
            if (blackistedVariable == null)
            {
                throw new InvalidOperationException("Attempting to persist null entity.");
            }

            if (blackistedVariable.Id > 0)
            {
                throw new NotSupportedException("Attempting to insert duplicate item in the database");
            }

            var queryString = @"
                INSERT INTO EnvironmentVariableBlacklist (Name, InitialValue, IsWindowsDefault, IsEnabled, AddedOn)
                VALUES (?, ?, ?, ?, ?)
";
            using (var connection = GetConnection())
            {
                var transaction = await connection.BeginTransactionAsync();
                await connection.ExecuteAsync(queryString,
                    new
                    {
                        blackistedVariable.Name,
                        blackistedVariable.InitialValue,
                        blackistedVariable.IsWindowsDefault,
                        blackistedVariable.IsEnabled, 
                        AddedOn = DateTime.Now
                    });

                var insertedId = await connection.QueryAsync<int>("SELECT last_insert_rowid()");
                var list = insertedId.ToList();
                if (list.Count == 1)
                {
                    blackistedVariable.Id = list.First();
                    await transaction.CommitAsync();
                }
                else
                {
                    throw new DataException("An unknown error occurred when trying to save the entity!");
                }

                return blackistedVariable;
            }
        }

        public async Task<IEnumerable<ColumnInfo>> GetAllColumnInfos()
        {
            return await GetColumnInfoBySettingNameAsync(null);
        }

        public IEnumerable<ColumnInfo> GetColumnInfoBySettingName(string settingName)
        {
            return GetColumnInfoBySettingNameAsync(settingName).GetAwaiter().GetResult();
        }

        public async Task<IEnumerable<ColumnInfo>> GetColumnInfoBySettingNameAsync(string settingName)
        {
            StringBuilder sb = new StringBuilder();
            object queryParam = null;

            
            const string sql = @"
                SELECT Id, IsVisible, ColumnWidth, Binding, OrdinalPosition, Header, Converter, ConverterParameter, SettingName
                FROM ColumnInfos
                WHERE 1 = 1
            ";
            
            sb.Append(sql);

            if (settingName != null)
            {
                queryParam = new {settingName};
                sb.Append("AND SettingName = ?");
            }

            using (var connection = GetConnection())
            {
                return await connection.QueryAsync<ColumnInfo>(sb.ToString(), queryParam);
            }
        }

        public ColumnInfo UpdateColumnInfo(ColumnInfo columnInfo)
        {
            return UpdateColumnInfoAsync(columnInfo).GetAwaiter().GetResult();
        } 

        public async Task<ColumnInfo> UpdateColumnInfoAsync(ColumnInfo columnInfo)
        {
            var sql = @"
                UPDATE ColumnInfos
                SET SettingName = ?,
                    IsVisible = ?,
                    ColumnWidth = ?,
                    Binding = ?,
                    OrdinalPosition = ?,
                    Header = ?,
                    Converter = ?,
                    ConverterParameter = ?
                WHERE Id = ?
";
            var param = new
            {
                columnInfo.SettingName,
                columnInfo.IsVisible,
                columnInfo.ColumnWidth,
                columnInfo.Binding,
                columnInfo.OrdinalPosition,
                columnInfo.Header,
                columnInfo.Converter,
                columnInfo.ConverterParameter
            };

            using var connection = GetConnection();
            var transaction = await connection.BeginTransactionAsync();
            await connection.ExecuteAsync(sql, param, transaction);
            
            var newColumnInfo = await GetColumnInfoByIdAsync(columnInfo.Id);

            await transaction.CommitAsync();

            return newColumnInfo;
        }

        public async Task<ColumnInfo> GetColumnInfoByIdAsync(int id)
        {
            var sql =
                @"SELECT Id, IsVisible, ColumnWidth, Binding, OrdinalPosition, Header, Converter, ConverterParameter, SettingName
                FROM ColumnInfos
                WHERE Id = ?";

            using var connection = GetConnection();

            return await connection.QuerySingleAsync<ColumnInfo>(sql, new {id});
        }

        public async Task<EnvironmentVariableBlacklist> UpdateBlacklistedVariableAsync(
            EnvironmentVariableBlacklist blacklistedVariable)
        {
            if (blacklistedVariable == null)
            {
                throw new InvalidOperationException("Attempting to update null entity.");
            }

            if (blacklistedVariable.Id == 0)
            {
                throw new NotSupportedException("Attempting to update non-persisted entity");
            }

            var queryString = @"
                UPDATE EnvironmentVariableBlacklist
                SET Name = ?,
                    InitialValue = ?,
                    IsWindowsDefault = ?,
                    IsEnabled = ?
                WHERE Id = ?
";
            using (var connection = GetConnection())
            {
                var transaction = await connection.BeginTransactionAsync();
                await connection.ExecuteAsync(queryString, new
                {
                    blacklistedVariable.Name, 
                    blacklistedVariable.InitialValue,
                    blacklistedVariable.IsWindowsDefault, 
                    blacklistedVariable.IsEnabled,
                    blacklistedVariable.Id
                });

                await transaction.CommitAsync();
                return blacklistedVariable;
            }
        }
    }
}