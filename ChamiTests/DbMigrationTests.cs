using System.Data.SQLite;
using ChamiDbMigrations;
using Xunit;

namespace ChamiTests
{
    public class DbMigrationTests
    {
        private static string migrationsPath = "D:/code/Chami/ChamiTests/InputFiles/Migrations";
        private static string connectionString = "Data Source=D:/code/Chami/ChamiTests/InputFiles/chami.db;Version=3;";

        [Fact]
        public void TestCollector()
        {
            /*
             * The collector is now an internal class
             */
            /*var collector = new DatabaseMigrationsCollector(migrationsPath);
            
            Assert.NotEmpty(collector.Collect());
            Assert.Equal("0001_initial.sql", collector.Collect()[0].Filename);
            Assert.Equal("0002_add_index.sql", collector.Collect()[1].Filename);*/
        }

        [Fact]
        public void TestMigration()
        {
            var connection = new SQLiteConnection(connectionString);
            var executor = new DatabaseMigrationExecutor(connection, migrationsPath);
            executor.Migrate();
        }
    }
}