using System.Data.SQLite;
using System.Threading.Tasks;

namespace Chami.Db.Repositories
{
    public class RepositoryBase
    {
        protected string ConnectionString { get; set; }
        protected SQLiteConnection GetConnection()
        {
            var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            return connection;
        }

        protected async Task<SQLiteConnection> GetConnectionAsync()
        {
            return await new Task<SQLiteConnection>(() => new SQLiteConnection(ConnectionString));
        }
    }
}