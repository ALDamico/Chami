using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Validators
{
    public class EnvironmentVariableValidator:IValidator<EnvironmentVariableViewModel>
    {
        public IValidationResult Validate(EnvironmentVariableViewModel environmentVariable)
        {
            var validationResult = new ValidationResult();
            validationResult.IsValid = true;
            if (environmentVariable == null)
            {
                validationResult.IsValid = false;
                validationResult.Message = "The entity was null.";
                return validationResult;
            }

            if (string.IsNullOrWhiteSpace(environmentVariable.Value))
            {
                validationResult.IsValid = false;
                validationResult.Message = "The Value attribute was null!";
            }

            if (string.IsNullOrWhiteSpace(environmentVariable.Name))
            {
                validationResult.IsValid = false;
                validationResult.Message = "The Name attribute was null!";
            }

            return validationResult;
        }
    }
}