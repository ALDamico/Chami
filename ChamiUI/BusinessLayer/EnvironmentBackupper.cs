using System;
using System.Collections;
using Chami.Db.Entities;
using Chami.Db.Repositories;
using Environment = Chami.Db.Entities.Environment;

namespace ChamiUI.BusinessLayer
{
    /// <summary>
    /// Backups the current Windows environment to the datastore *without* including the environment variables added by Chami.
    /// </summary>
    public static class EnvironmentBackupper
    {
        /// <summary>
        /// Removes the environment variables added by Chami from a dictionary.
        /// </summary>
        /// <param name="environmentVariables">A dictionary containing all environment variables.</param>
        /// <param name="repository">An <see cref="EnvironmentRepository"/> with which to query the datastore.</param>
        /// <returns>The input environmentVariables parameter without the variables added by Chami.</returns>
        private static IDictionary RemoveCurrentChamiVariables(IDictionary environmentVariables, EnvironmentRepository repository)
        {
            var currentEnvironmentName = environmentVariables["_CHAMI_ENV"];
            if (currentEnvironmentName != null)
            {
                var currentEnvironment = repository.GetEnvironmentByName(currentEnvironmentName.ToString());
                if (currentEnvironment != null)
                {
                    foreach (var variable in currentEnvironment.EnvironmentVariables)
                    {
                        var variableName = variable.Name;
                        environmentVariables.Remove(variableName);
                    }
                }
            }
            environmentVariables.Remove("_CHAMI_ENV");
            return environmentVariables;
        }

        /// <summary>
        /// Backs up the current environment to the datastore, excluding the environment variables added by Chami.
        /// </summary>
        /// <param name="repository">An <see cref="EnvironmentRepository"/> where to save the backup.</param>
        public static void Backup(EnvironmentRepository repository)
        {
            var environmentVariables = System.Environment.GetEnvironmentVariables();

            environmentVariables = RemoveCurrentChamiVariables(environmentVariables, repository);

            var backupEnvironment = new Environment();

            var environmentName = $"Backup of {DateTime.Now:s}";
            backupEnvironment.Name = environmentName;
            backupEnvironment.EnvironmentType = EnvironmentType.BackupEnvironment;

            foreach (DictionaryEntry entry in environmentVariables)
            {
                var variableName = (string)entry.Key;
                var variableValue = (string)entry.Value;
                var environmentVariable = new EnvironmentVariable { Name = variableName, Value = variableValue };
                backupEnvironment.EnvironmentVariables.Add(environmentVariable);
            }

            repository.InsertEnvironment(backupEnvironment);
        }
    }
}