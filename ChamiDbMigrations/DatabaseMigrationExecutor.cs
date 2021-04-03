using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace ChamiDbMigrations
{
    public class DatabaseMigrationExecutor
    {
        public DatabaseMigrationExecutor([NotNull] IDbConnection connection, string migrationsPath)
        {
            _connection = connection;
            _migrations = new List<DatabaseMigration>();
            _collector = new DatabaseMigrationsCollector(migrationsPath);
        }
        private IDbConnection _connection;

        private List<DatabaseMigration> _migrations;
        private DatabaseMigrationsCollector _collector;

        public void Migrate()
        {
            if (_migrations.Count == 0)
            {
                _migrations = _collector.Collect();
            }

            foreach (var migration in _migrations)
            {
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();                    
                }
                var command = _connection.CreateCommand();
                var fileContent = File.OpenText(migration.FullPath).ReadToEnd();

                command.CommandText = fileContent;
                var result = command.ExecuteNonQuery();
            }
        }
    }
}