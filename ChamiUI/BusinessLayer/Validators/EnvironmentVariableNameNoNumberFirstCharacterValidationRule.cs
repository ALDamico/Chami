using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using ChamiUI.Localization;

namespace ChamiUI.BusinessLayer.Validators
{
    public class EnvironmentVariableNameNoNumberFirstCharacterValidationRule:ValidationRule
    {
        public override System.Windows.Controls.ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var environmentVariableViewModel =  ValidationUtils.ConvertObjectToValidate(value);
            if (environmentVariableViewModel != null)
            {
                environmentVariableViewModel.IsValid = false;
                var name = environmentVariableViewModel.Name;
                if (name == null)
                {
                    environmentVariableViewModel.IsValid = true;
                    return  System.Windows.Controls.ValidationResult.ValidResult;
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