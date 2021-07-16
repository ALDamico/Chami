using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Validators
{
    /// <summary>
    /// Performs simple validation on <see cref="EnvironmentVariableViewModel"/> objects when converting between entities and viewmodels.
    /// </summary>
    public class EnvironmentVariableValidator:IValidator<EnvironmentVariableViewModel>
    {
        /// <summary>
        /// Performs simple validation on <see cref="EnvironmentVariableViewModel"/> objects when converting between entities and viewmodels.
        /// This method checks that the viewmodel is not null, that it has a name and value, but features no deeper tests.
        /// </summary>
        /// <param name="environmentVariable">The <see cref="EnvironmentVariableViewModel"/> to validate.</param>
        /// <returns>If all checks pass, return a valid validation result. Otherwise, returns an invalid validation result.</returns>
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

            /*if (string.IsNullOrWhiteSpace(environmentVariable.Value))
            {
                validationResult.IsValid = false;
                validationResult.Message = "The Value attribute was null!";
            }*/

            if (string.IsNullOrWhiteSpace(environmentVariable.Name))
            {
                validationResult.IsValid = false;
                validationResult.Message = "The Name attribute was null!";
            }

            return validationResult;
        }
    }
}