using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Validators
{
    public class EnvironmentVariableNameUniqueValidationRule:ValidationRule 
    {
        public CollectionViewSource EnvironmentVariables { get; set; }
        public override System.Windows.Controls.ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var environmentVariable = ValidationUtils.ConvertObjectToValidate(value);
            var environmentVariableCollection = EnvironmentVariables.Source as IEnumerable<EnvironmentVariableViewModel>;
            if (environmentVariable != null)
            {
                if (environmentVariableCollection?.Count(v => v.Name == environmentVariable.Name) > 1)
                {
                    var errorMessage = ChamiUIStrings.EnvironmentVariableNameNotUniqueErrorMessage;
                    var environmentName = environmentVariable.Environment?.Name;
                    errorMessage = string.Format(errorMessage, environmentVariable.Name, environmentName);
                    return new System.Windows.Controls.ValidationResult(false, errorMessage);
                }
            }
            return System.Windows.Controls.ValidationResult.ValidResult;
        }
    }
}