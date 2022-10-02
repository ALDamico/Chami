using System.Globalization;
using System.Windows.Controls;
using ChamiUI.BusinessLayer.Validators.Enums;

namespace ChamiUI.BusinessLayer.Validators;

public class FontSizeValidationRule : ValidationRule
{
    public double FontSize { get; set; }
    public FontSizeType FontSizeType { get; set; }
    public override System.Windows.Controls.ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value is double doubleValue)
        {
            return ValidateInner(doubleValue);
        }
        return System.Windows.Controls.ValidationResult.ValidResult;
    }

    private System.Windows.Controls.ValidationResult ValidateInner(double value)
    {
        if (FontSizeType == FontSizeType.Min)
        {
            if (value < FontSize)
            {
                return new System.Windows.Controls.ValidationResult(false, "Font size too small"); // TODO Convert to resources
            }
        }
        else
        {
            if (value > FontSize)
            {
                return new System.Windows.Controls.ValidationResult(false, "Font size too large"); // TODO Convert to resources
            }
        }
        
        return System.Windows.Controls.ValidationResult.ValidResult;
    }
}