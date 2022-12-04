using System.Globalization;
using System.Windows.Controls;

namespace ChamiUI.BusinessLayer.Validators;

public abstract class AbstractChamiValidationRule : ValidationRule
{
    public AbstractChamiValidationRule(ValidationRuleTarget target)
    {
        Target = target;
    }
    public ValidationRuleTarget Target { get;  }
    public override System.Windows.Controls.ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        throw new System.NotImplementedException();
    }
}