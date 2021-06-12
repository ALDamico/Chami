using System.Globalization;
using System.Windows.Controls;
using ChamiUI.Localization;

namespace ChamiUI.BusinessLayer.Validators
{
    public class EnvironmentVariableNameLengthValidationRule:ValidationRule
    {
        public int MaxLength { get; set; }
        public override System.Windows.Controls.ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var environmentVariable = ValidationUtils.ConvertObjectToValidate(value);
            if (environmentVariable != null)
            {
                environmentVariable.IsValid = true;
                var name = environmentVariable.Name;
                if (name != null)
                {
                    if (name.Length > MaxLength)
                    {
                        environmentVariable.IsValid = false;
                        var errorMessage = string.Format(ChamiUIStrings.EnvironmentVariableNameLengthErrorMessage,
                            MaxLength, name.Length);
                        return new System.Windows.Controls.ValidationResult(false, errorMessage);
                    }
                }
            }
            
            return System.Windows.Controls.ValidationResult.ValidResult;
        }
    }
}