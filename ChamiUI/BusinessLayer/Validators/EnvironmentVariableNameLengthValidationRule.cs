using System.Globalization;
using System.Windows.Controls;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Validators
{
    /// <summary>
    /// Validates the length of the Name property of an <see cref="EnvironmentVariableViewModel"/>.
    /// The maximum allowed length is configurable through the <see cref="MaxLength"/> property, but Chami always uses
    /// Windows' default value of 2047.
    /// https://devblogs.microsoft.com/oldnewthing/20100203-00/?p=15083
    /// </summary>
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