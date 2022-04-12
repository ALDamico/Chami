using System.Globalization;
using System.Windows.Controls;
using ChamiUI.Localization;

namespace ChamiUI.BusinessLayer.Validators
{
    /// <summary>
    /// Validates the name of an environment.
    /// </summary>
    public class EnvironmentNameValidationRule : ValidationRule
    {
        /// <summary>
        /// Validates the name of an environment.
        /// Such a name can't be null, an empty string or a string composed of only spaces.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="cultureInfo">Not used.</param>
        /// <returns>If the value parameter is not a string, is a null string, is an empty string or a string composed of only whitespace, returns a non-valid result. Otherwise, returns <see cref="System.Windows.Controls.ValidationResult.ValidResult"/>.</returns>
        /// <seealso cref="System.Windows.Controls.ValidationResult"/>
        public override System.Windows.Controls.ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string environmentName && !string.IsNullOrWhiteSpace(environmentName))
            {
                return System.Windows.Controls.ValidationResult.ValidResult;
            }

            return new System.Windows.Controls.ValidationResult(false,
                ChamiUIStrings.EnvironmentNameValidationErrorMessage);
        }
    }
}