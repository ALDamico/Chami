using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer;

public class EnvironmentPropertiesFileReader : IEnvironmentReader<EnvironmentViewModel>
{
    private readonly string _inputFile;

    public EnvironmentPropertiesFileReader(string inputFile)
    {
        _inputFile = inputFile;
    }
    public EnvironmentViewModel Process()
    {
        var environmentViewModel = new EnvironmentViewModel();
        var fileContent = File.ReadAllLines(_inputFile);

        foreach (var row in fileContent)
        {
            if (string.IsNullOrWhiteSpace(row)) continue;

            if (Regex.IsMatch(row, @"^\s*#")) continue;
            
            var split = row.Split("=");

            var name = split[0];
            var value = split[1];
            
            if (split.Length > 2)
            {
                value = string.Join("=", split.Skip(1));
            }

            var variable = new EnvironmentVariableViewModel()
                {Name = name, Value = value, Environment = environmentViewModel};
            environmentViewModel.EnvironmentVariables.Add(variable);
        }

        return environmentViewModel;
    }

    public ICollection<EnvironmentViewModel> ProcessMultiple()
    {
        throw new NotSupportedException(ChamiUIStrings.FeatureNotSupportedExceptionMessage);
    }
}