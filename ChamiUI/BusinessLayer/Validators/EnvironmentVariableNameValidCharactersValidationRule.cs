using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using ChamiUI.Localization;
using Microsoft.VisualBasic;

namespace ChamiUI.BusinessLayer.Validators
{
    public class EnvironmentVariableNameValidCharactersValidationRule : ValidationRule
    {
        public override System.Windows.Controls.ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var environmentVariable = ValidationUtils.ConvertObjectToValidate(value);
            if (environmentVariable != null)
            {
                var environmentVariableName = environmentVariable.Name;
                if (Regex.IsMatch(environmentVariableName, "^[A-Za-z0-9_]*$"))
                {
                    return System.Windows.Controls.ValidationResult.ValidResult;
                }

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
                    errorMessage = Strings.Format(ChamiUIStrings.EnvironmentVariableNameInvalidCharactersErrorMessageSingular, positionString);
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