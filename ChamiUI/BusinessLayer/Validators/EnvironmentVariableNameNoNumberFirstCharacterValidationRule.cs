using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using ChamiUI.Localization;

namespace ChamiUI.BusinessLayer.Validators
{
    /// <summary>
    /// Validates that an environment variable name doesn't start with a number.
    /// </summary>
    public class EnvironmentVariableNameNoNumberFirstCharacterValidationRule : AbstractChamiValidationRule
    {
        public EnvironmentVariableNameNoNumberFirstCharacterValidationRule():base(ValidationRuleTarget.Self)
        {
            
        }
        /// <summary>
        /// Validates that an environment variable name doesn't start with a number.
        /// </summary>
        /// <param name="value">The environment variable to validate.</param>
        /// <param name="cultureInfo">Unused.</param>
        /// <returns>A <see cref="System.Windows.Controls.ValidationResult.ValidResult"/> if the environment variable name doesn't start with a number, otherwise an invalid result.</returns>
        public override System.Windows.Controls.ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var environmentVariableViewModel = ValidationUtils.ConvertObjectToValidate(value);
            if (environmentVariableViewModel != null)
            {
                environmentVariableViewModel.IsValid = false;
                var name = environmentVariableViewModel.Name;
                if (name == null)
                {
                    environmentVariableViewModel.IsValid = true;
                    return System.Windows.Controls.ValidationResult.ValidResult;
                }

                if (Regex.IsMatch(name, "^[0-9]"))
                {
                    return new System.Windows.Controls.ValidationResult(false,
                        ChamiUIStrings.EnvironmentVariableNameNumberFirstCharacterErrorMessage);
                }
            }

            return System.Windows.Controls.ValidationResult.ValidResult;
        }
    }
}