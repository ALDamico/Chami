namespace ChamiUI.BusinessLayer.Validators
{
    /// <summary>
    /// Basic implementation of the <see cref="IValidationResult"/> interface.
    /// </summary>
    public class ValidationResult : IValidationResult
    {
        /// <summary>
        /// Constructs a new <see cref="ValidationResult"/> object with the <see cref="IsValid"/> property set to false and the <see cref="Message"/> property set to null.
        /// </summary>
        public ValidationResult()
        {

        }

        /// <summary>
        /// Constructs a new <see cref="ValidationResult"/> object and sets the objects properties with the values passed in as parameters.
        /// </summary>
        /// <param name="isValid">The value to assign to the <see cref="IsValid"/> property.</param>
        /// <param name="message">The value to assign to the <see cref="Message"/> property.</param>
        public ValidationResult(bool isValid, string message)
        {
            IsValid = isValid;
            Message = message;
        }
        /// <summary>
        /// Determines if the validation succeeded or not.
        /// </summary>
        /// <seealso cref="IValidationResult.IsValid"/>
        public bool IsValid { get; set; }
        /// <summary>
        /// A descriptive message for the validation result.
        /// </summary>
        /// <seealso cref="IValidationResult.Message"/>
        public string Message { get; set; }
    }
}