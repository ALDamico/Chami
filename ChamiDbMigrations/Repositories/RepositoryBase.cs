using System;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace Chami.Db.Repositories
{
    /// <summary>
    /// Base class for all repositories in the Chami application.
    /// </summary>
    public class RepositoryBase
    {
        /// <summary>
        /// The connection string for the database to connect to.
        /// </summary>
        protected string ConnectionString { get; set; }

        /// <summary>
        /// Creates a new database connection and opens it.
        /// </summary>
        /// <returns>The newly-created connection.</returns>
        protected SQLiteConnection GetConnection()
        {
            var connection = new SQLiteConnection(ConnectionString);
            connection.SetExtendedResultCodes(true);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// Creates a new database connection asynchronously.
        /// </summary>
        /// <returns>The newly-created connection.</returns>
        [Obsolete("This async overload is obsolete", true)]
        protected async Task<SQLiteConnection> GetConnectionAsync()
        {
            throw new NotSupportedException("Not supported");
        }
    }
}