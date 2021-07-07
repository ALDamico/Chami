using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Validators
{
    /// <summary>
    /// Validates that an environment variable name is unique in an environment.
    /// </summary>
    public class EnvironmentVariableNameUniqueValidationRule:ValidationRule 
    {
        /// <summary>
        /// The set of <see cref="EnvironmentVariableViewModel"/>s to validate the target against.
        /// </summary>
        public CollectionViewSource EnvironmentVariables { get; set; }
        /// <summary>
        /// Validates that an environment variable name is unique in an environment.
        /// </summary>
        /// <param name="value">The <see cref="EnvironmentVariableViewModel"/> to validate.</param>
        /// <param name="cultureInfo">Unused</param>
        /// <returns>If there are no other environment variables in <see cref="EnvironmentVariables"/>, returns a valid result. Otherwise, returns an invalid result.</returns>
        public override System.Windows.Controls.ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var environmentVariable = ValidationUtils.ConvertObjectToValidate(value);
            var environmentVariableCollection = EnvironmentVariables.Source as IEnumerable<EnvironmentVariableViewModel>;
            if (environmentVariable != null)
            {
                environmentVariable.IsValid = true;
                if (environmentVariable.MarkedForDeletion)
                {
                    return System.Windows.Controls.ValidationResult.ValidResult;
                }
                
                if (environmentVariableCollection?.Count(v => v.Name == environmentVariable.Name) > 1)
                {
                    if (environmentVariableCollection.Any(
                        v => v.MarkedForDeletion && v.Name == environmentVariable.Name))
                    {
                        return System.Windows.Controls.ValidationResult.ValidResult;
                    }
                    var errorMessage = ChamiUIStrings.EnvironmentVariableNameNotUniqueErrorMessage;
                    var environmentName = environmentVariable.Environment?.Name;
                    errorMessage = string.Format(errorMessage, environmentVariable.Name, environmentName);
                    environmentVariable.IsValid = false;
                    return new System.Windows.Controls.ValidationResult(false, errorMessage);
                }
            }
            return System.Windows.Controls.ValidationResult.ValidResult;
        }
    }
}