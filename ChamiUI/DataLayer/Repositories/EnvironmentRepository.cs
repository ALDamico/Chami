using System;
using System.Data.SQLite;
using Dapper;
using Environment = ChamiUI.DataLayer.Entities.Environment;

namespace ChamiUI.DataLayer.Repositories
{
    public class EnvironmentRepository
    {
        public EnvironmentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private string _connectionString;

        public Environment GetEnvironmentById(int id)
        {
            var queryString = @"
                SELECT e.EnvironmentId, e.Name, e.AddedOn, ev.EnvironmentVariableId, ev.Name, ev.AddedOn, ev.Value, ev.EnvironmentId
                FROM Environments e
                JOIN EnvironmentVariables ev on e.EnvironmentId = ev.EnvironmentId
                WHERE e.EnvironmentId = ?
 ";
            using (var connection = GetConnection())
            {
                try
                {
                    return connection.QuerySingle<Environment>(queryString, new {id});
                }
                catch (InvalidOperationException)
                {
                    return null;
                }
            }
        }

        public Environment InsertEnvironment(Environment environment)
        {
            if (environment == null)
            {
                return null;
            }

            if (environment.EnvironmentId > 0)
            {
                throw new NotSupportedException("Attempting to insert duplicate item in the database");
            }

            var queryString = @"INSERT INTO Environments(Name, AddedON) VALUES (?, ?)";
            using (var connection = GetConnection())
            {
                connection.Execute(queryString, new {environment.Name, environment.AddedOn});
                var environmentVariableInsertQuery = @"
                INSERT INTO EnvironmentVariables(Name, Value, AddedOn, EnvironmentId)
                VALUES (?, ?, ?, ?)
";
                var selectQuery = @"
                    SELECT * 
                    FROM Environments e 
                    LEFT JOIN EnvironmentVariables ev ON ev.EnvironmentId = e.EnvironmentId 
                    WHERE e.AddedOn = ?";
                var result = connection.QuerySingle<Environment>(selectQuery, new {environment.AddedOn});
                environment.EnvironmentId = result.EnvironmentId;
                foreach (var environmentVariable in environment.EnvironmentVariables)
                {
                    environmentVariable.EnvironmentId = environment.EnvironmentId;
                    connection.Execute(environmentVariableInsertQuery,
                        new
                        {
                            environmentVariable.Name, environmentVariable.Value, environmentVariable.AddedOn,
                            environmentVariable.EnvironmentId
                        });
                }
            }

            return environment;
        }

        public SQLiteConnection GetConnection()
        {
            return new(_connectionString);
        }

        public Environment UpdateEnvironment(Environment environment)
        {
            if (environment.EnvironmentId == 0)
            {
                throw new NotSupportedException("Attempting to update an entity that has not been persisted.");
            }

            var updateQuery = @"
                UPDATE Environments
                SET Name = ?
                WHERE EnvironmentId = ?
";
            using (var connection = GetConnection())
            {
                connection.Execute(updateQuery, new {environment.Name, environment.EnvironmentId});
                foreach (var environmentVariable in environment.EnvironmentVariables)
                {
                    var envVarUpdateQuery = @"
                    UPDATE EnvironmentVariables 
                    SET Name = ?
                        Value = ?
                        EnvironmentId = ?
                    WHERE EnvironmentVariableId = ?
";
                    var updObj = new
                    {
                        environmentVariable.Name, environmentVariable.Value, environmentVariable.EnvironmentId,
                        environmentVariable.EnvironmentVariableId
                    };

                    connection.Execute(envVarUpdateQuery, updObj);
                }
            }


            var updatedEnvironment = GetEnvironmentById(environment.EnvironmentId);
            return updatedEnvironment;
        }
    }
}