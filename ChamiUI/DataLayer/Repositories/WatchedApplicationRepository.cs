using ChamiUI.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ChamiUI.DataLayer.Repositories
{
    public class WatchedApplicationRepository: RepositoryBase
    {
        public WatchedApplicationRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public IEnumerable<WatchedApplication> GetWatchedApplications()
        {
            var sql = @"
                SELECT Id, Name, IsWatchEnabled
                FROM WatchedApplications
";
            using (var connection = GetConnection())
            {
                return connection.Query<WatchedApplication>(sql);
            }
        }

        public WatchedApplication GetWatchedApplicationById(int id)
        {
            var sql = @"
                SELECT Id, Name, IsWatchEnabled
                FROM WatchedApplications
                WHERE Id = ?
";
            using (var connection = GetConnection())
            {
                return connection.QuerySingle<WatchedApplication>(sql, new { id });
            }
        }

        public IEnumerable<WatchedApplication> GetActiveWatchedApplications()
        {
            var sql = @"
                SELECT Id, Name, IsWatchEnabled
                FROM WatchedApplications
                WHERE IsWatchEnabled = 1
";
            using (var connection = GetConnection())
            {
                return connection.Query<WatchedApplication>(sql);
            }
        }

        public WatchedApplication InsertWatchedApplication(WatchedApplication application)
        {
            if (application == null)
            {
                return null;
            }

            if (application.Id > 0)
            {
                throw new NotSupportedException("Attempting to insert duplicate item in the database");
            }

            var sql = @"
                INSERT INTO WatchedApplications(Name, IsWatchEnabled)
                VALUES (?, ?)
";
            using (var connection = GetConnection())
            {
                connection.Execute(sql, new { application.Name, application.IsWatchEnabled });
            }

            application = GetWatchedApplicationByName(application.Name);

            return application;
        }

        public WatchedApplication UpdateApplication(WatchedApplication application)
        {
            if (application == null)
            {
                return null;
            }

            if (application.Id == 0)
            {
                throw new NotSupportedException("Attempting to update an entity that hasn't been persisted.");
            }

            var sql = @"
                UPDATE WatchedApplication
                SET Name = ?,
                    IsWatchEnabled = ?
                WHERE Id = ?
";
            using (var connection = GetConnection())
            {
                connection.Execute(sql, new { application.Name, application.IsWatchEnabled, application.Id });
            }

            return application;
        }

        public WatchedApplication GetWatchedApplicationByName(string name)
        {
            var sql = @"
                SELECT Id, Name, IsWatchEnabled
                FROM WatchedApplications
                WHERE Name = ?
";
            using (var connection = GetConnection())
            {
                return connection.QuerySingle<WatchedApplication>(sql, new { name });
            }
        }
    }
}
