using System;
using System.Collections;
using Chami.Db.Entities;
using Chami.Db.Repositories;
using Environment = Chami.Db.Entities.Environment;

namespace ChamiUI.BusinessLayer
{
    public static class EnvironmentBackupper
    {
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

        public static void Backup(EnvironmentRepository repository)
        {
            var environmentVariables = System.Environment.GetEnvironmentVariables();

            environmentVariables = RemoveCurrentChamiVariables(environmentVariables, repository);

            var backupEnvironment = new Environment();

            var environmentName = $"Backup of {DateTime.Now:s}";
            backupEnvironment.Name = environmentName;
            backupEnvironment.IsBackup = true;

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