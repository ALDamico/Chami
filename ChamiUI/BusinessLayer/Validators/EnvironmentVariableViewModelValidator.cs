using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Validators
{
    public class EnvironmentVariableViewModelValidator : IValidator<EnvironmentVariableViewModel>
    {
        public IValidationResult Validate(EnvironmentVariableViewModel viewModel)
        {
            ValidationResult result = new ValidationResult();
            result.IsValid = true;

            if (viewModel == null)
            {
                result.IsValid = false;
                result.Message = "The object was null!";
            }
            else if (string.IsNullOrWhiteSpace(viewModel.Name))
            {
                result.IsValid = false;
                result.Message = "The name was null!";
            }
            else if (string.IsNullOrWhiteSpace(viewModel.Value))
            {
                result.IsValid = false;
                result.Message = "The value was null!";
            }

            return result;
        }
    }
}