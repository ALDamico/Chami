using ChamiUI.PresentationLayer.ViewModels;
using System;

namespace ChamiUI.BusinessLayer.Validators
{
    /// <summary>
    /// Performs basic validation on <see cref="EnvironmentViewModel"/> objects.
    /// </summary>
    public class EnvironmentViewModelValidator : IValidator<EnvironmentViewModel>
    {
        /// <summary>
        /// Validates that the environment's name is not null.
        /// </summary>
        /// <param name="viewModel">The <see cref="EnvironmentViewModel"/> object to validate.</param>
        /// <returns>If the <see cref="EnvironmentViewModel"/> parameter Name property is valid, returns a valid result. Otherwise, returns an invalid one.</returns>
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