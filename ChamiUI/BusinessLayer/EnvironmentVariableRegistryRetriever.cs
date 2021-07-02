using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace ChamiUI.BusinessLayer
{
    /// <summary>
    /// Retrieves information about environment variables using the Windows registry.
    /// </summary>
    public class EnvironmentVariableRegistryRetriever
    {
        /// <summary>
        /// Gets all environment variables as a <see cref="Dictionary{TKey,TValue}"/>-
        /// </summary>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> where the key is the variable name and the value is the variable value.</returns>
        public Dictionary<string, string> GetEnvironmentVariables()
        {
            var variableNames = GetVariableNames();
            var keyName = @$"{CurrentUser}\{SubKey}";
            var output = new Dictionary<string, string>();

            foreach (var name in variableNames)
            {
                var value = Registry.GetValue(keyName, name, "");
                var unboxed = (string) value;
                output.Add(name, unboxed);
            }

            return output;
        }

        private const string CurrentUser = "HKEY_CURRENT_USER";
        private const string SubKey = "Environment";

        /// <summary>
        /// Gets the value of a single environment variable by its name.
        /// </summary>
        /// <param name="name">The name of the environment variable to retrieve.</param>
        /// <returns>If a value is found, returns the corresponding value, otherwise null.</returns>
        public string GetEnvironmentVariable(string name)
        {
            var keyName = @$"{CurrentUser}\{SubKey}";

            return (string) Registry.GetValue(keyName, name, null);
        }

        /// <summary>
        /// Gets all the environment variable names.
        /// </summary>
        /// <returns>An array of strings containing the names of all environment variables for the current user.</returns>
        private string[] GetVariableNames()
        {
            var list = new List<string>();
            var registryKey = Registry.CurrentUser.OpenSubKey(SubKey);
            if (registryKey != null)
            {
                return registryKey.GetValueNames();
            }

            return null;
        }
    }
}