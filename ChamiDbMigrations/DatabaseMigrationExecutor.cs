using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
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
            
            OpenConnection();

            long lastAppliedMigration = -1;

            var migrationsTableExists = CheckMigrationsTableExists();
            if (!migrationsTableExists)
            {
                CreateMigrationsTable();
            }
            else
            {
                lastAppliedMigration = GetLastMigration();
            }

            foreach (var migration in _migrations)
            {
                if ( migration.Order <= lastAppliedMigration)
                {
                    continue;
                }
                var sql = File.OpenText(migration.FullPath).ReadToEnd();
                var command = _connection.CreateCommand();
                
                command.CommandText = sql;
                var result = command.ExecuteNonQuery();
                InsertMigration(migration);
            }
        }

        private void CreateMigrationsTable()
        {
            OpenConnection();

            var command = _connection.CreateCommand();
            command.CommandText = @"CREATE TABLE ChamiMigrations (""Order"" INTEGER NOT NULL, Name TEXT)";
            command.ExecuteNonQuery();
        }

        private bool CheckMigrationsTableExists()
        {
            OpenConnection();

            var command = _connection.CreateCommand();
            command.CommandText = @"SELECT 1 FROM ChamiMigrations";
            try
            {
                command.ExecuteReader();
            }
            catch (DbException)
            {
                return false;
            }

            return true;
        }

        private void OpenConnection()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        private void InsertMigration(DatabaseMigration migration)
        {
            var sql = @"
                INSERT INTO ChamiMigrations(""Order"", Name)
                VALUES (?, ?)
";
            var command = _connection.CreateCommand();
            command.CommandText = sql;
            var orderParameter = SQLiteParameterFactory.Convert(migration.Order);
            var nameParameter = SQLiteParameterFactory.Convert(migration.Filename);
            
            command.Parameters.Add(orderParameter);
            command.Parameters.Add(nameParameter);
            command.ExecuteNonQuery();
        }

        private long GetLastMigration()
        {
            OpenConnection();
            var query = @"
                SELECT MAX(""Order"")
                FROM ChamiMigrations
";
            var command = _connection.CreateCommand();
            command.CommandText = query;
            var result = command.ExecuteScalar();
            if (result != DBNull.Value)
            {
                return (long) result;
            }

            return -1;
        }
    }
}