using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using ChamiUI.Localization;

namespace ChamiUI.BusinessLayer.Validators
{
    public class EnvironmentNameValidationRule:ValidationRule
    {
        public override System.Windows.Controls.ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string environmentName)
            {
                if (string.IsNullOrWhiteSpace(environmentName))
                {
                    return new System.Windows.Controls.ValidationResult(false,
                        ChamiUIStrings.EnvironmentNameValidationErrorMessage);
                }
            }
            return System.Windows.Controls.ValidationResult.ValidResult;
        }
    }
}