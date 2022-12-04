using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Controls;
using ChamiUI.BusinessLayer.Validators;
using ValidationResult = System.Windows.Controls.ValidationResult;

namespace ChamiUI.PresentationLayer.ViewModels;

public abstract class AbstractValidatableViewModel : ViewModelBase, IDataErrorInfo
{
    static AbstractValidatableViewModel()
    {
        PropertyValidationMap = new Dictionary<PropertyInfo, List<ValidationRule>>();
    }
    public string Error { get; protected set; }

    public string this[string columnName]
    {
        get
        {
            Error = null;
            PropertyInfo pInfo = GetPropertyInfoByName(columnName);
            if (pInfo == null)
            {
                return null;
            }

            bool hasValidationRules = PropertyValidationMap.TryGetValue(pInfo, out var validationRules);
            if (!hasValidationRules)
            {
                return null;
            }
            var errors = new List<string>();
            foreach (var validationRule in validationRules)
            {
                var validationResult = InvokeRule(pInfo, validationRule);
                if (!validationResult.IsValid)
                {
                    errors.Add(validationResult.ErrorContent.ToString());
                }
            }

            Error = string.Join(System.Environment.NewLine, errors);
            return Error;
        }
    }

    private ValidationResult InvokeRule(PropertyInfo propertyInfo, ValidationRule validationRule)
    {
        object objectToValidate = this;
        if (validationRule is AbstractChamiValidationRule { Target: ValidationRuleTarget.Property })
        {
            objectToValidate = propertyInfo.GetValue(this);
        }
        
        return validationRule.Validate(objectToValidate, CultureInfo.InvariantCulture);
    }
    protected static Dictionary<PropertyInfo, List<ValidationRule>> PropertyValidationMap { get; private set; }

    protected static void AddValidationRule(PropertyInfo propertyInfo, ValidationRule validationRule)
    {
        var keyExists = PropertyValidationMap.ContainsKey(propertyInfo);
        if (!keyExists)
        {
            PropertyValidationMap[propertyInfo] = new List<ValidationRule>();
            
        }
        
        PropertyValidationMap[propertyInfo].Add(validationRule);
    }

    protected static void AddValidationRules(PropertyInfo propertyInfo, IEnumerable<ValidationRule> validationRules)
    {
        foreach (var validationRule in validationRules)
        {
            AddValidationRule(propertyInfo, validationRule);
        }
    }

    private PropertyInfo GetPropertyInfoByName(string columnName)
    {
        return GetType().GetProperty(columnName);
    }

    public List<ValidationResult> Validate()
    {
        var validationResults = new List<ValidationResult>();
        foreach (var keyValuePair in PropertyValidationMap)
        {
            var property = keyValuePair.Key;
            var validationRules = keyValuePair.Value;
            foreach (var validationRule in validationRules)
            {
                var validationResult = InvokeRule(property, validationRule);
                validationResults.Add(validationResult);
            }
        }

        return validationResults;
    }
}