using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using ChamiUI.DataLayer.Entities;
using Dapper;
using Environment = ChamiUI.DataLayer.Entities.Environment;

namespace ChamiUI.DataLayer.Repositories
{
    public class EnvironmentRepository:RepositoryBase
    {
        public EnvironmentRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public async Task<Environment> GetEnvironmentByIdAsync(int id)
        {
            var queryString = @"
                SELECT *
                FROM Environments e
                JOIN EnvironmentVariables ev on e.EnvironmentId = ev.EnvironmentId
                WHERE e.EnvironmentId = ?
 ";
            using (var connection = await GetConnectionAsync())
            {
                var environmentDictionary = new Dictionary<int, Environment>();
                try
                {
                    var param = new {id};
                    var result = await connection.QueryAsync<Environment, EnvironmentVariable, Environment>(queryString,
                        (e, v) =>
                        {
                            Environment env;

                            if (!environmentDictionary.TryGetValue(e.EnvironmentId, out env))
                            {
                                env = e;
                                environmentDictionary[e.EnvironmentId] = e;
                            }

                            v.Environment = e;
                            environmentDictionary[e.EnvironmentId].EnvironmentVariables.Add(v);
                            return env;
                        }, param, splitOn: "EnvironmentVariableId");
                    return result.FirstOrDefault();
                }
                catch (InvalidOperationException)
                {
                    return null;
                }
            }
        }

        public Environment GetEnvironmentById(int id)
        {
            var queryString = @"
                SELECT *
                FROM Environments e
                JOIN EnvironmentVariables ev on e.EnvironmentId = ev.EnvironmentId
                WHERE e.EnvironmentId = ?
 ";
            using (var connection = GetConnection())
            {
                var environmentDictionary = new Dictionary<int, Environment>();
                try
                {
                    var param = new {id};
                    var result = connection.Query<Environment, EnvironmentVariable, Environment>(queryString,
                        (e, v) =>
                        {
                            Environment env;

                            if (!environmentDictionary.TryGetValue(e.EnvironmentId, out env))
                            {
                                env = e;
                                environmentDictionary[e.EnvironmentId] = e;
                            }

                            v.Environment = e;
                            environmentDictionary[e.EnvironmentId].EnvironmentVariables.Add(v);
                            return env;
                        }, param, splitOn: "EnvironmentVariableId");
                    return result.FirstOrDefault();
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
                var transaction = connection.BeginTransaction();
                connection.Execute(queryString, new {environment.Name, environment.AddedOn});
                var environmentVariableInsertQuery = @"
                INSERT INTO EnvironmentVariables(Name, Value, AddedOn, EnvironmentId)
                VALUES (?, ?, ?, ?)
";
                var selectQuery = @"
                    SELECT * 
                    FROM Environments e
                    WHERE e.AddedOn = ?";
                var result = connection.QuerySingle<Environment>(selectQuery, new {environment.AddedOn});
                environment.EnvironmentId = result.EnvironmentId;
                foreach (var environmentVariable in environment.EnvironmentVariables)
                {
                    environmentVariable.EnvironmentId = environment.EnvironmentId;
                    connection.Execute(environmentVariableInsertQuery,
                        new
                        {
                            Name = environmentVariable.Name, environmentVariable.Value,
                            environmentVariable.AddedOn,
                            environmentVariable.EnvironmentId
                        });
                }
                transaction.Commit();
            }
            
            return environment;
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
                    SET Name = ?,
                        Value = ?
                    WHERE EnvironmentVariableId = ?
";
                    var updObj = new
                    {
                        Name = environmentVariable.Name, environmentVariable.Value,
                        environmentVariable.EnvironmentId,
                        environmentVariable.EnvironmentVariableId
                    };

                    connection.Execute(envVarUpdateQuery, updObj);
                }
            }

            var updatedEnvironment = GetEnvironmentById(environment.EnvironmentId);
            return updatedEnvironment;
        }

        public Environment UpsertEnvironment(Environment environment)
        {
            UpdateEnvironment(environment);

            var newVariables = environment.EnvironmentVariables.Where(v => v.EnvironmentVariableId == 0);

            foreach (var environmentVariable in newVariables)
            {
                InsertVariable(environmentVariable, environment.EnvironmentId);
            }

            return GetEnvironmentById(environment.EnvironmentId);
        }

        protected void InsertVariable(EnvironmentVariable environmentVariable, int environmentId)
        {
            var environmentVariableInsertQuery = @"
                INSERT INTO EnvironmentVariables(Name, Value, AddedOn, EnvironmentId)
                VALUES (?, ?, ?, ?)
";
            using (var connection = GetConnection())
            {
                var updObj = new
                {
                    Name = environmentVariable.Name, environmentVariable.Value,
                    environmentVariable.AddedOn,
                    environmentId
                };
                connection.Execute(environmentVariableInsertQuery, updObj);
            }
        }

        public ICollection<Environment> GetEnvironments()
        {
            var queryString = @"
                SELECT *
                FROM Environments
                LEFT JOIN EnvironmentVariables ON Environments.EnvironmentId = EnvironmentVariables.EnvironmentId
";
            using (var connection = GetConnection())
            {
                var dict = new Dictionary<int, Environment>();
                var result = connection.Query<Environment, EnvironmentVariable, Environment>(queryString, (e, v) =>
                {
                    Environment env;

                    if (!dict.TryGetValue(e.EnvironmentId, out env))
                    {
                        env = e;
                        dict[e.EnvironmentId] = e;
                    }

                    if (v != null)
                    {
                        v.Environment = e;
                        dict[e.EnvironmentId].EnvironmentVariables.Add(v);
                    }


                    return env;
                }, splitOn: "EnvironmentVariableId");
                return dict.Values.ToList();
            }
        }

        public Environment GetEnvironmentByName(string name)
        {
            var queryString = @"
                SELECT *
                FROM Environments e
                LEFT JOIN EnvironmentVariables ev on e.EnvironmentId = ev.EnvironmentId
                WHERE e.Name = ?
 ";
            using (var connection = GetConnection())
            {
                var environmentDictionary = new Dictionary<int, Environment>();
                try
                {
                    var param = new {name};
                    var result = connection.Query<Environment, EnvironmentVariable, Environment>(queryString,
                        (e, v) =>
                        {
                            Environment env;

                            if (!environmentDictionary.TryGetValue(e.EnvironmentId, out env))
                            {
                                env = e;
                                environmentDictionary[e.EnvironmentId] = e;
                            }

                            if (v != null)
                            {
                                v.Environment = e;
                                environmentDictionary[e.EnvironmentId].EnvironmentVariables.Add(v);
                            }
                            
                            
                            return env;
                        }, param, splitOn: "EnvironmentVariableId");
                    return result.FirstOrDefault();
                }
                catch (InvalidOperationException)
                {
                    return null;
                }
            }
        }

        public bool DeleteEnvironmentById(int id)
        {
            var queryString = @"
                DELETE FROM EnvironmentVariables
                WHERE EnvironmentId = ?
";
            using (var connection = GetConnection())
            {
                connection.Execute(queryString, new {id});

                queryString = @"
                    DELETE FROM Environments
                WHERE EnvironmentId = ?
";
                var result = connection.Execute(queryString, new {id});
                return result > 0;
            }
        }
    }
}