using System.Globalization;
using System.Windows.Controls;
using ChamiUI.Localization;

namespace ChamiUI.BusinessLayer.Validators
{
    /// <summary>
    /// Validates that the Name property of an EntironmentVariableViewModel object is not null, empty, or only composed of whitespace characters.
    /// </summary>
    public class EnvironmentVariableNameNotNullValidationRule:ValidationRule
    {
        /// <summary>
        /// Validates that the Name property of an EntironmentVariableViewModel object is not null, empty, or only composed of whitespace characters.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="cultureInfo">Unused.</param>
        /// <returns>If the Name property is null, an empty string or only contains whitespace characters, returns an invalid result. Otherwise, a valid result.</returns>
        public override System.Windows.Controls.ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var environmentVariable = ValidationUtils.ConvertObjectToValidate(value);
            if (environmentVariable != null)
            {
                environmentVariable.IsValid = true;
                if (string.IsNullOrWhiteSpace(environmentVariable.Name))
                {
                    environmentVariable.IsValid = false;
                    return new System.Windows.Controls.ValidationResult(false,
                        ChamiUIStrings.EnvironmentVariableNameNotNullErrorMessage);
                }
            }
            return System.Windows.Controls.ValidationResult.ValidResult;
        }
    }
}