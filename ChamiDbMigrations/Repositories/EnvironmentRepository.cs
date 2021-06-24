using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chami.Db.Entities;
using Dapper;
using Environment = Chami.Db.Entities.Environment;

namespace Chami.Db.Repositories
{
    public class EnvironmentRepository : RepositoryBase
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
                LEFT JOIN EnvironmentVariables ev on e.EnvironmentId = ev.EnvironmentId
                WHERE e.EnvironmentId = ?
 ";
            using (var connection = await GetConnectionAsync())
            {
                var environmentDictionary = new Dictionary<int, Environment>();
                try
                {
                    var param = new { id };
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
                SELECT DISTINCT *
                FROM Environments e
                LEFT JOIN EnvironmentVariables ev on e.EnvironmentId = ev.EnvironmentId
                WHERE e.EnvironmentId = ?
 ";
            using (var connection = GetConnection())
            {
                var environmentDictionary = new Dictionary<int, Environment>();
                try
                {
                    var param = new { id };
                    var result = connection.Query<Environment, EnvironmentVariable, Environment>(queryString,
                        (e, v) =>
                        {
                            Environment env;

                            if (!environmentDictionary.TryGetValue(e.EnvironmentId, out env))
                            {
                                env = e;
                                environmentDictionary[e.EnvironmentId] = e;
                            }

                            // Can happen if the environment has no variables attached to it.
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

            var queryString = @"INSERT INTO Environments(Name, AddedOn, EnvironmentType) VALUES (?, ?, ?)";
            using (var connection = GetConnection())
            {
                var transaction = connection.BeginTransaction();
                connection.Execute(queryString, new { environment.Name, environment.AddedOn, environment.EnvironmentType });
                var environmentVariableInsertQuery = @"
                INSERT INTO EnvironmentVariables(Name, Value, AddedOn, EnvironmentId)
                VALUES (?, ?, ?, ?)
";
                var selectQuery = @"
                    SELECT * 
                    FROM Environments e
                    WHERE e.AddedOn = ?";
                var results = connection.Query<Environment>(selectQuery, new { environment.AddedOn });
                var result = results.FirstOrDefault();
                environment.EnvironmentId = result.EnvironmentId;
                foreach (var environmentVariable in environment.EnvironmentVariables)
                {
                    environmentVariable.EnvironmentId = environment.EnvironmentId;
                    connection.Execute(environmentVariableInsertQuery,
                        new
                        {
                            Name = environmentVariable.Name,
                            environmentVariable.Value,
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
                connection.Execute(updateQuery, new { environment.Name, environment.EnvironmentId });
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
                        Name = environmentVariable.Name,
                        Value = environmentVariable.Value,
                        EnvironmentVariableId = environmentVariable.EnvironmentVariableId
                    };

                    connection.Execute(envVarUpdateQuery, updObj);
                }
            }

            var updatedEnvironment = GetEnvironmentById(environment.EnvironmentId);
            return updatedEnvironment;
        }

        public Environment UpsertEnvironment(Environment environment)
        {
            if (environment.EnvironmentId == 0)
            {
                return InsertEnvironment(environment);
            }
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
            if (environmentVariable == null)
            {
                return;
            }

            if (environmentVariable.Name == null)
            {
                throw new InvalidOperationException("Attempting to insert an entity with NULL Name");
            }

            if (environmentVariable.Value == null)
            {
                throw new InvalidOperationException("Attempting to insert an entity with NULL Value");
            }
            var environmentVariableInsertQuery = @"
                INSERT INTO EnvironmentVariables(Name, Value, AddedOn, EnvironmentId)
                VALUES (?, ?, ?, ?)
";
            using (var connection = GetConnection())
            {
                var updObj = new
                {
                    Name = environmentVariable.Name,
                    environmentVariable.Value,
                    environmentVariable.AddedOn,
                    environmentId
                };
                connection.Execute(environmentVariableInsertQuery, updObj);
            }
        }

        public ICollection<Environment> GetBackupEnvironments()
        {
            var queryString = @"
                SELECT *
                FROM Environments
                LEFT JOIN EnvironmentVariables ON Environments.EnvironmentId = EnvironmentVariables.EnvironmentId
                WHERE EnvironmentType = 1
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

        public ICollection<Environment> GetEnvironments()
        {
            var queryString = @"
                SELECT *
                FROM Environments
                LEFT JOIN EnvironmentVariables ON Environments.EnvironmentId = EnvironmentVariables.EnvironmentId
                WHERE EnvironmentType = 0
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
                    var param = new { name };
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
                connection.Execute(queryString, new { id });

                queryString = @"
                    DELETE FROM Environments
                WHERE EnvironmentId = ?
";
                var result = connection.Execute(queryString, new { id });
                return result > 0;
            }
        }

        public void DeleteVariableById(int selectedVariableId)
        {
            var queryString = @"
                DELETE FROM EnvironmentVariables
                WHERE EnvironmentVariableId = ?
";
            using (var connection = GetConnection())
            {
                connection.Execute(queryString, new {selectedVariableId});
            }
        }
    }
}