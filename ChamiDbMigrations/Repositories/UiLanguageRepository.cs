using System.Collections.Generic;
using Chami.Db.Entities;
using Dapper;

namespace Chami.Db.Repositories
{
    /// <summary>
    /// Performs CRUD operations on the <see cref="UiLanguage"/> aggregate.
    /// </summary>
    public class UiLanguageRepository : RepositoryBase
    {
        /// <summary>
        /// Constructs a new <see cref="UiLanguageRepository"/> with the requested connection string.
        /// </summary>
        /// <param name="connectionString">The connection string to connect to the database.</param>
        public UiLanguageRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Gets all the <see cref="UiLanguage"/>s available to the Chami application.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> with all the available <see cref="UiLanguage"/>s.</returns>
        public IEnumerable<UiLanguage> GetAllUiLanguages()
        {
            var queryString = @"
                SELECT Code, Name, FlagPath
                FROM UiLanguages
";
            using var connection = GetConnection();
            var result = connection.Query<UiLanguage>(queryString);
            return result;
        }

        /// <summary>
        /// Gets a <see cref="UiLanguage"/> with the requested code.
        /// </summary>
        /// <param name="code">The ISO-639 code of the requested language.</param>
        /// <returns>If a suitable <see cref="UiLanguage"/> exists in the database, returns it. Otherwise, null.</returns>
        public UiLanguage GetUiLanguageByCode(string code)
        {
            var queryString = @"
                SELECT Code, Name, FlagPath
                FROM UiLanguages
                WHERE Code = ?
";
            using var connection = GetConnection();
            var result = connection.QuerySingle<UiLanguage>(queryString, new {Code = code});
            return result;
        }
    }
}