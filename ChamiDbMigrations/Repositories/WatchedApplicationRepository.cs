using System;
using System.Collections.Generic;
using Chami.Db.Entities;
using Dapper;

namespace Chami.Db.Repositories
{
    /// <summary>
    /// Performs CRUD operations on the <see cref="WatchedApplication"/> aggregate.
    /// </summary>
    public class WatchedApplicationRepository : RepositoryBase
    {
        /// <summary>
        /// Constructs a new <see cref="WatchedApplicationRepository"/> object with the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string to initialize the object with.</param>
        public WatchedApplicationRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Gets all the <see cref="WatchedApplication"/> objects available to the Chami application.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> containing all the <see cref="WatchedApplication"/> objects.</returns>
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

        /// <summary>
        /// Gets a <see cref="WatchedApplication"/> object with the specified id.
        /// </summary>
        /// <param name="id">The id of the object to retrieve.</param>
        /// <returns>If a matching <see cref="WatchedApplication"/> is found, returns it. Otherwise, null.</returns>
        public WatchedApplication GetWatchedApplicationById(int id)
        {
            var sql = @"
                SELECT Id, Name, IsWatchEnabled
                FROM WatchedApplications
                WHERE Id = ?
";
            using (var connection = GetConnection())
            {
                return connection.QuerySingle<WatchedApplication>(sql, new {id});
            }
        }

        /// <summary>
        /// Gets all the <see cref="WatchedApplication"/> objects that Chami is currently listening to.
        /// </summary>
        /// <returns>A (possibly empty) <see cref="IEnumerable{T}"/> of <see cref="WatchedApplication"/> objects.</returns>
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

        /// <summary>
        /// Inserts a new <see cref="WatchedApplication"/> object in the database.
        /// </summary>
        /// <param name="application">The new <see cref="WatchedApplication"/> to insert.</param>
        /// <returns>If the parameter was null, returns null. If the insertion operation was successful, returns the newly-inserted <see cref="WatchedApplication"/> object.</returns>
        /// <exception cref="NotSupportedException">If the <see cref="WatchedApplication"/>'s Id is greater than 0 (i.e., if it's already been persisted, throws a <see cref="NotSupportedException"/>.</exception>
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
                connection.Execute(sql, new {application.Name, application.IsWatchEnabled});
            }

            application = GetWatchedApplicationByName(application.Name);

            return application;
        }

        /// <summary>
        /// Updates an existing <see cref="WatchedApplication"/> object in the database.
        /// </summary>
        /// <param name="application">The application object to update.</param>
        /// <returns>If the parameter is null, returns null. Otherwise, returns the updated entity.</returns>
        /// <exception cref="NotSupportedException">The <see cref="WatchedApplication"/> has not yet been persisted.</exception>
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
                UPDATE WatchedApplications
                SET Name = ?,
                    IsWatchEnabled = ?
                WHERE Id = ?
";
            using (var connection = GetConnection())
            {
                connection.Execute(sql, new {application.Name, application.IsWatchEnabled, application.Id});
            }

            return application;
        }

        /// <summary>
        /// Gets the <see cref="WatchedApplication"/> with the specified name.
        /// </summary>
        /// <param name="name">The name of the application to retrieve.</param>
        /// <returns>If a suitable object exists in the database, returns it. Otherwise, null.</returns>
        public WatchedApplication GetWatchedApplicationByName(string name)
        {
            var sql = @"
                SELECT Id, Name, IsWatchEnabled
                FROM WatchedApplications
                WHERE Name = ?
";
            using (var connection = GetConnection())
            {
                return connection.QuerySingle<WatchedApplication>(sql, new {name});
            }
        }
    }
}