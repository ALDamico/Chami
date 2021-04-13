using ChamiUI.PresentationLayer.ViewModels;
using System;

namespace ChamiUI.BusinessLayer.Validators
{
    public class EnvironmentViewModelValidator : IValidator<EnvironmentViewModel>
    {
        public IValidationResult Validate(EnvironmentViewModel viewModel)
        {
            ValidationResult result = new ValidationResult();
            result.IsValid = true;
            if (String.IsNullOrWhiteSpace(viewModel.Name))
            {
                result.IsValid = false;
                result.Message = "Must specify a name!";
            }

            return result;
        }
    }
}