using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Validators;

public static class DatagridValidationRulesFactory
{
    public static List<ValidationRule> GetDatagridValidationRules(
        CollectionViewSource environmentVariables)
    {
        var validationRules = new List<ValidationRule>();

        validationRules.Add(new ExceptionValidationRule()
            {ValidationStep = ValidationStep.UpdatedValue, ValidatesOnTargetUpdated = true});
        validationRules.Add(new EnvironmentVariableNameValidCharactersValidationRule()
            {ValidationStep = ValidationStep.UpdatedValue, ValidatesOnTargetUpdated = true});
        validationRules.Add(new EnvironmentVariableNameLengthValidationRule()
            {ValidationStep = ValidationStep.UpdatedValue, ValidatesOnTargetUpdated = true, MaxLength = 2047});
        validationRules.Add(new EnvironmentVariableNameNoNumberFirstCharacterValidationRule()
            {ValidationStep = ValidationStep.UpdatedValue, ValidatesOnTargetUpdated = true});
        validationRules.Add(new EnvironmentVariableNameUniqueValidationRule()
        {
            ValidationStep = ValidationStep.CommittedValue, ValidatesOnTargetUpdated = true,
            EnvironmentVariables = environmentVariables
        });

        return validationRules;
    }
}