using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Validators
{
    public class EnvironmentVariableNameNotNullValidationRule:ValidationRule
    {
        public override System.Windows.Controls.ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var environmentVariable = ValidationUtils.ConvertObjectToValidate(value);
            if (environmentVariable != null)
            {
                if (string.IsNullOrWhiteSpace(environmentVariable.Name))
                {
                    return new System.Windows.Controls.ValidationResult(false,
                        ChamiUIStrings.EnvironmentVariableNameNotNullErrorMessage);
                }
            }
            return System.Windows.Controls.ValidationResult.ValidResult;
        }
    }
}