using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Chami.Db.Entities;
using Dapper;
using Environment = Chami.Db.Entities.Environment;

namespace Chami.Db.Repositories
{
    /// <summary>
    /// Performs CRUD operations on the Environment aggregate.
    /// </summary>
    public class EnvironmentRepository : RepositoryBase
    {
        /// <summary>
        /// Constructs a new <see cref="EnvironmentRepository"/> object and sets the appropriate connection string.
        /// </summary>
        /// <param name="connectionString"></param>
        public EnvironmentRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Retrieves the <see cref="Environment"/> with the specified id and its <see cref="EnvironmentVariable"/>s asynchronously.
        /// </summary>
        /// <param name="id">The id of the <see cref="Environment"/> to retrieve.</param>
        /// <returns>If a match is found, returns the corresponding <see cref="Environment"/>, otherwise null.</returns>
        public async Task<Environment> GetEnvironmentByIdAsync(int id)
        {
            var queryString = @"
                SELECT *
                FROM Environments e
                LEFT JOIN EnvironmentVariables ev on e.EnvironmentId = ev.EnvironmentId
                WHERE e.EnvironmentId = ?
 ";
            using (var connection = GetConnection())
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

        /// <summary>
        /// Gets the environment with the specified Id and its corresponding <see cref="EnvironmentVariable"/>s.
        /// </summary>
        /// <param name="id">The id of the <see cref="Environment"/> to retrieve.</param>
        /// <returns>The <see cref="Environment"/> with the Id specified. If none is found, returns null.</returns>
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

        /// <summary>
        /// Inserts an <see cref="Environment"/> object in the datastore and its associated <ses cref="EnvironmentVariable"/>s.
        /// </summary>
        /// <param name="environment">The <see cref="Environment"/> object to insert into the datastore.</param>
        /// <returns>The newly-inserted environment, with its EnvironmentId set. If the parameter is null, returns null.</returns>
        /// <exception cref="NotSupportedException">If the EnvironmentId is greater than 0 (i.e., if the <see cref="Environment"/> is already persisted to the datastore), a <see cref="NotSupportedException"/> is thrown.</exception>
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
                connection.Execute(queryString,
                    new { environment.Name, environment.AddedOn, environment.EnvironmentType });
                var environmentVariableInsertQuery = @"
                INSERT INTO EnvironmentVariables(Name, Value, AddedOn, EnvironmentId, IsFolder)
                VALUES (?, ?, ?, ?, ?)
";
                var selectQuery = @"
                    SELECT * 
                    FROM Environments e
                    WHERE e.AddedOn = ?";
                var results = connection.Query<Environment>(selectQuery, new { environment.AddedOn });
                var result = results.FirstOrDefault();
                if (result != null)
                {
                    environment.EnvironmentId = result.EnvironmentId;
                }

                var environmentVariables = new List<EnvironmentVariable>(environment.EnvironmentVariables);
                environment.EnvironmentVariables.Clear();

                foreach (var environmentVariable in environmentVariables)
                {
                    if (!environmentVariable.MarkedForDeletion)
                    {
                        environmentVariable.EnvironmentId = environment.EnvironmentId;
                        connection.Execute(environmentVariableInsertQuery,
                            new
                            {
                                environmentVariable.Name,
                                environmentVariable.Value,
                                environmentVariable.AddedOn,
                                environmentVariable.EnvironmentId,
                                environmentVariable.IsFolder
                            });
                        
                        environment.EnvironmentVariables.Add(environmentVariable);
                    }
                }

                transaction.Commit();
            }
            
            

            return environment;
        }


        /// <summary>
        /// Updates the data associated with an <see cref="Environment"/> in the datastore.
        /// </summary>
        /// <param name="environment">The <see cref="Environment"/> to update.</param>
        /// <returns>The updated <see cref="Environment"/> entity.</returns>
        /// <exception cref="NotSupportedException">If the <see cref="Environment"/> is not yet persisted (i.e., its EnvironmentId is 0), a <see cref="NotSupportedException"/> is thrown.</exception>
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
                        Value = ?,
                        IsFolder = ?
                    WHERE EnvironmentVariableId = ?
";
                    var updObj = new
                    {
                        environmentVariable.Name,
                        environmentVariable.Value,
                        environmentVariable.IsFolder,
                        environmentVariable.EnvironmentVariableId
                    };

                    connection.Execute(envVarUpdateQuery, updObj);
                }
            }

            var updatedEnvironment = GetEnvironmentById(environment.EnvironmentId);
            return updatedEnvironment;
        }

        /// <summary>
        /// Inserts an <see cref="Environment"/> in the datastore, or updates it if it's already present.
        /// </summary>
        /// <param name="environment">The <see cref="Environment"/> to persist in the datastore.</param>
        /// <returns>The newly-inserted or updated <see cref="Environment"/></returns>
        public Environment UpsertEnvironment(Environment environment)
        {
            var environmentInDatabase = GetEnvironmentByName(environment.Name);
            if (environmentInDatabase != null)
            {
                environment.EnvironmentId = environmentInDatabase.EnvironmentId;
                foreach (var environmentVariable in environment.EnvironmentVariables)
                {
                    var correspondingVariable =
                        environmentInDatabase.EnvironmentVariables.FirstOrDefault(v =>
                            v.Name.Equals(environmentVariable.Name));
                    if (correspondingVariable != null)
                    {
                        environmentVariable.EnvironmentId = environment.EnvironmentId;
                        environmentVariable.EnvironmentVariableId = correspondingVariable.EnvironmentVariableId;
                    }
                }
            }

            if (environment.EnvironmentId == 0)
            {
                return InsertEnvironment(environment);
            }

            UpdateEnvironment(environment);

            var newVariables = environment.EnvironmentVariables.Where(v => v.EnvironmentVariableId == 0 && !v.MarkedForDeletion);

            foreach (var environmentVariable in newVariables)
            {
                InsertVariable(environmentVariable, environment.EnvironmentId);
            }

            foreach (var oldEnvironmentVariable in environment.EnvironmentVariables)
            {
                if (oldEnvironmentVariable.MarkedForDeletion)
                {
                    DeleteVariableById(oldEnvironmentVariable.EnvironmentVariableId);
                }
            }

            return GetEnvironmentById(environment.EnvironmentId);
        }

        /// <summary>
        /// Inserts a new <see cref="EnvironmentVariable"/> object in the datastore.
        /// </summary>
        /// <param name="environmentVariable">The <see cref="EnvironmentVariable"/> to insert.</param>
        /// <param name="environmentId">The Id of the <see cref="Environment"/> to attach the new variable to.</param>
        /// <exception cref="InvalidOperationException">If the <see cref="EnvironmentVariable"/> object's Name or Value attributes is null, an <see cref="InvalidOperationException"/> is thrown to preserve the consistency of the datastore.</exception>
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
                INSERT INTO EnvironmentVariables(Name, Value, AddedOn, EnvironmentId, IsFolder)
                VALUES (?, ?, ?, ?, ?)
";
            using (var connection = GetConnection())
            {
                var updObj = new
                {
                    environmentVariable.Name,
                    environmentVariable.Value,
                    environmentVariable.AddedOn,
                    environmentId,
                    environmentVariable.IsFolder
                };
                connection.Execute(environmentVariableInsertQuery, updObj);
            }
        }

        /// <summary>
        /// Gets all the <see cref="Environment"/> objects in the datastore marked as "Backup environments".
        /// </summary>
        /// <returns>An <see cref="ICollection{T}"/> of <see cref="Environment"/> objects.</returns>
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
                connection.Query<Environment, EnvironmentVariable, Environment>(queryString, (e, v) =>
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

        private ICollection<Environment> GetEnvironmentsByType(EnvironmentType type)
        {
            var queryString = @"
                SELECT *
                FROM Environments
                LEFT JOIN EnvironmentVariables ON Environments.EnvironmentId = EnvironmentVariables.EnvironmentId
                LEFT JOIN Categories ON Categories.Id = Environments.CategoryId
                WHERE EnvironmentType = ?
";
            using (var connection = GetConnection())
            {
                var dict = new Dictionary<int, Environment>();
                connection.Query<Environment, EnvironmentVariable, Environment>(queryString, (e, v) =>
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
                }, splitOn: "EnvironmentVariableId", param: new {type});
                return dict.Values.ToList();
            }
        }

        /// <summary>
        /// Get all the environments marked as normal environments in the datastore.
        /// </summary>
        /// <returns>An <see cref="ICollection{Environment}"/> containing all the environments in the datastore.</returns>
        public ICollection<Environment> GetTemplateEnvironments()
        {
            return GetEnvironmentsByType(EnvironmentType.TemplateEnvironment);
        }

        /// <summary>
        /// Get all the environments marked as normal environments in the datastore.
        /// </summary>
        /// <returns>An <see cref="ICollection{Environment}"/> containing all the environments in the datastore.</returns>
        public ICollection<Environment> GetEnvironments()
        {
            return GetEnvironmentsByType(EnvironmentType.NormalEnvironment);
        }

        /// <summary>
        /// Get the <see cref="Environment"/> with the specified name and its associated <see cref="EnvironmentVariable"/>s.
        /// </summary>
        /// <param name="name">The name of the <see cref="Environment"/> to retrieve</param>
        /// <returns>An <see cref="Environment"/> with the specified name. If none is found, null.</returns>
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

        /// <summary>
        /// Deletes the <see cref="Environment"/> with the specified id from the database and cascades the delete to its corresponsing <see cref="EnvironmentVariable"/>s.
        /// </summary>
        /// <param name="id">The Id of the <see cref="Environment"/> to delete.</param>
        /// <returns>True if the deletion was successful, false if no <see cref="Environment"/> with the requested id was found.</returns>
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

        /// <summary>
        /// Deletes the <see cref="EnvironmentVariable"/> with the specified id from the datastore.
        /// </summary>
        /// <param name="selectedVariableId">The id of the <see cref="EnvironmentVariable"/> to delete.</param>
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

        public async Task<IEnumerable<EnvironmentVariableBlacklist>> GetBlacklistedVariablesAsync()
        {
            var queryString = @"
                SELECT *
                FROM EnvironmentVariableBlacklist
";

            using (var connection = GetConnection())
            {
                return await connection.QueryAsync<EnvironmentVariableBlacklist>(queryString);
            }
        }

        public async Task<EnvironmentVariableBlacklist> InsertBlacklistedVariableAsync(
            EnvironmentVariableBlacklist blackistedVariable)
        {
            if (blackistedVariable == null)
            {
                throw new InvalidOperationException("Attempting to persist null entity.");
            }

            if (blackistedVariable.Id > 0)
            {
                throw new NotSupportedException("Attempting to insert duplicate item in the database");
            }

            var queryString = @"
                INSERT INTO EnvironmentVariableBlacklist (Name, InitialValue, IsWindowsDefault, IsEnabled, AddedOn)
                VALUES (?, ?, ?, ?, ?)
";
            using (var connection = GetConnection())
            {
                var transaction = await connection.BeginTransactionAsync();
                await connection.ExecuteAsync(queryString,
                    new
                    {
                        blackistedVariable.Name,
                        blackistedVariable.InitialValue,
                        blackistedVariable.IsWindowsDefault,
                        blackistedVariable.IsEnabled, 
                        AddedOn = DateTime.Now
                    });

                var insertedId = await connection.QueryAsync<int>("SELECT last_insert_rowid()");
                var list = insertedId.ToList();
                if (list.Count() == 1)
                {
                    blackistedVariable.Id = list.First();
                    await transaction.CommitAsync();
                }
                else
                {
                    throw new DataException("An unknown error occurred when trying to save the entity!");
                }

                return blackistedVariable;
            }
        }

        public async Task<EnvironmentVariableBlacklist> UpdateBlacklistedVariableAsync(
            EnvironmentVariableBlacklist blacklistedVariable)
        {
            if (blacklistedVariable == null)
            {
                throw new InvalidOperationException("Attempting to update null entity.");
            }

            if (blacklistedVariable.Id == 0)
            {
                throw new NotSupportedException("Attempting to update non-persisted entity");
            }

            var queryString = @"
                UPDATE EnvironmentVariableBlacklist
                SET Name = ?,
                    InitialValue = ?,
                    IsWindowsDefault = ?,
                    IsEnabled = ?
                WHERE Id = ?
";
            using (var connection = GetConnection())
            {
                var transaction = await connection.BeginTransactionAsync();
                await connection.ExecuteAsync(queryString, new
                {
                    // ReSharper disable once RedundantAnonymousTypePropertyName
                    Name = blacklistedVariable.Name, 
                    // ReSharper disable once RedundantAnonymousTypePropertyName
                    InitialValue = blacklistedVariable.InitialValue,
                    // ReSharper disable once RedundantAnonymousTypePropertyName
                    IsWindowsDefault = blacklistedVariable.IsWindowsDefault, 
                    // ReSharper disable once RedundantAnonymousTypePropertyName
                    IsEnabled = blacklistedVariable.IsEnabled,
                    // ReSharper disable once RedundantAnonymousTypePropertyName
                    Id = blacklistedVariable.Id
                });

                await transaction.CommitAsync();
                return blacklistedVariable;
            }
        }

        public async Task<EnvironmentVariableBlacklist> UpsertBlacklistedVariableAsync(
            EnvironmentVariableBlacklist blacklistedVariable)
        {
            if (blacklistedVariable == null)
            {
                throw new InvalidOperationException("Attempting to persist null entity.");
            }

            if (blacklistedVariable.Id == 0)
            {
                return await InsertBlacklistedVariableAsync(blacklistedVariable);
            }

            return await UpdateBlacklistedVariableAsync(blacklistedVariable);
        }

        public Task<IEnumerable<string>> GetVariableNamesAsync()
        {
            var sql = @"
                SELECT DISTINCT ev.Name
                FROM EnvironmentVariables ev
";

            using var connection = GetConnection();
            return connection.QueryAsync<string>(sql);
        }

        public async Task UpdateVariableByNameAsync(string variableName, string variableValue)
        {
            var sql = GetUpdateVariableByNameQuery();
            var connection = GetConnection();
            await connection.ExecuteAsync(sql, new {variableValue, variableName});
        }

        private string GetUpdateVariableByNameQuery()
        {
            return @"
                UPDATE EnvironmentVariables
                SET Value = ?
                WHERE Name = ?
";
        }

        public async Task UpdateVariableByNameAndEnvironmentIdsAsync(string variableName, string variableValue, ImmutableList<int> environmentIds)
        {
            var environmentIdString = string.Join(',', environmentIds);
            var sql = GetUpdateVariableByNameQuery() + " AND EnvironmentId IN (?)";

            using var connection = GetConnection();

            await connection.ExecuteAsync(sql, new {variableValue, variableName, environmentIds});

        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            var query = @"
                SELECT Id, Name, BackgroundColor, Icon, Visibility
                FROM Categories
";
            using var connection = GetConnection();
            return await connection.QueryAsync<Category>(query);
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return GetAllCategoriesAsync().GetAwaiter().GetResult();
        }

        public async Task<Category> InsertCategoryAsync(Category category)
        {
            if (category.Id != 0)
            {
                throw new InvalidOperationException("Entity already persisted");
            }

            var query = @"
                INSERT INTO Categories(Name, BackgroundColor, Icon, Visibility)
                VALUES (?, ?, ?, ?)
";
            using var connection = GetConnection();
            var transaction = await connection.BeginTransactionAsync();
            var inserted = await connection.ExecuteAsync(query, category, transaction);

            if (inserted == 0)
            {
                await transaction.RollbackAsync();
                throw new SQLiteException("Entity not persisted");
            }

            var rowId = connection.LastInsertRowId;
            category.Id = (int)rowId;

            await transaction.CommitAsync();
            return category;
        }

        public Category InsertCategory(Category category)
        {
            return InsertCategoryAsync(category).GetAwaiter().GetResult();
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            if (category.Id == 0)
            {
                throw new InvalidOperationException("Entity not persisted");
            }
            var query = @"
                UPDATE Categories
                SET Name = ?,
                    BackgroundColor = ?,
                    Icon = ?,
                    Visibility = ?
                WHERE Id = ?
            ";

            using var connection = GetConnection();
            var result = await connection.ExecuteAsync(query, category);

            if (result == 0)
            {
                throw new InvalidOperationException("No rows affected");
            }

            return category;
        }

        public Category UpdateCategory(Category category)
        {
            return UpdateCategoryAsync(category).GetAwaiter().GetResult();
        }

        public async Task<Category> UpsertCategoryAsync(Category category)
        {
            if (category.Id != 0)
            {
                return await UpdateCategoryAsync(category);
            }

            return await InsertCategoryAsync(category);
        }

        public Category UpsertCategory(Category category)
        {
            return UpdateCategoryAsync(category).GetAwaiter().GetResult();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var query = @"
                SELECT Id, Name, BackgroundColor, Icon, Visibility
                FROM Categories 
                WHERE Id = ?
";
            using var connection = GetConnection();

            return await connection.QuerySingleOrDefaultAsync<Category>(query, id);
        }

        public Category GetCategoryById(int id)
        {
            return GetCategoryByIdAsync(id).GetAwaiter().GetResult();
        }
    }
}