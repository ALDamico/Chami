using System.Collections.Generic;
using Chami.Db.Entities;
using Dapper;

namespace Chami.Db.Repositories
{
    public class UiLanguageRepository : RepositoryBase
    {
        public UiLanguageRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

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