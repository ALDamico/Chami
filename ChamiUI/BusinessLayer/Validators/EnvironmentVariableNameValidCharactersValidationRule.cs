using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using Chami.Db.Entities;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Validators
{
    /// <summary>
    /// Validates that there are no invalid characters in an environment variable name.
    /// </summary>
    public class EnvironmentVariableNameValidCharactersValidationRule : ValidationRule
    {
        /// <summary>
        /// Validates that there are no invalid characters in an environment variable name.
        /// If the validation fails, the error message contains the list of invalid character positions.
        /// </summary>
        /// <param name="value">The <see cref="EnvironmentVariableViewModel"/> to validate.</param>
        /// <param name="cultureInfo">Unused.</param>
        /// <returns></returns>
        public override System.Windows.Controls.ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var environmentVariable = ValidationUtils.ConvertObjectToValidate(value);
            if (environmentVariable != null)
            {
                var environment = environmentVariable.Environment;
                if (environment != null && environment.EnvironmentType == EnvironmentType.BackupEnvironment)
                {
                    return System.Windows.Controls.ValidationResult.ValidResult;
                }
                var environmentVariableName = environmentVariable.Name;
                environmentVariable.IsValid = true;
                if (environmentVariableName == null)
                {
                    return System.Windows.Controls.ValidationResult.ValidResult;
                }
                if (Regex.IsMatch(environmentVariableName, "^[A-Za-z0-9_]*$"))
                {
                    return System.Windows.Controls.ValidationResult.ValidResult;
                }

                environmentVariable.IsValid = false;

                // Regex explanation
                // Match one or more characters not matching the characters in the intervals A-Z, a-z, 0-9 and the
                // character underscore and also isn't the end of the string.
                var invalidCharacterPositions = Regex.Matches(environmentVariableName, "(?![A-Za-z0-9_])+(?!$)");
                var positions = new List<int>();
                foreach (Match match in invalidCharacterPositions)
                {
                    positions.Add(match.Index);
                }

                var positionString = string.Join(", ", positions);
                string errorMessage;
                if (positions.Count == 1)
                {
                    errorMessage = string.Format(ChamiUIStrings.EnvironmentVariableNameInvalidCharactersErrorMessageSingular, positionString);
                }
                else
                {
                    errorMessage = string.Format(ChamiUIStrings.EnvironmentVariableNameInvalidCharactersErrorMessage,
                        positionString);
                }

                return new System.Windows.Controls.ValidationResult(false, errorMessage);
            }

            return System.Windows.Controls.ValidationResult.ValidResult;
        }
    }
}