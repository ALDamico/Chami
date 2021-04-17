using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace ChamiUI.BusinessLayer
{
    public class EnvironmentVariableRegistryRetriever
    {
        public Dictionary<string, string> GetEnvironmentVariables()
        {
            var variableNames = GetVariableNames();
            var keyName = @$"{_currentUser}\{_subKey}";
            var output = new Dictionary<string, string>();

            foreach (var name in variableNames)
            {
                var value = Registry.GetValue(keyName, name, "");
                var unboxed = (string) value;
                output.Add(name, unboxed);
            }

            return output;
        }

        private const string _currentUser = "HKEY_CURRENT_USER";
        private const string _subKey = "Environment";

        public string GetEnvironmentVariable(string name)
        {
            var keyName = @$"{_currentUser}\{_subKey}";

            return (string) Registry.GetValue(keyName, name, null);
        }

        private string[] GetVariableNames()
        {
            var list = new List<string>();
            var registryKey = Registry.CurrentUser.OpenSubKey(_subKey);
            if (registryKey != null)
            {
                return registryKey.GetValueNames();
            }

            return null;
        }
    }
}