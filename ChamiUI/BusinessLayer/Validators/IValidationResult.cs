namespace ChamiUI.BusinessLayer.Validators
{
    /// <summary>
    /// Interface for basic ValidationResult objects.
    /// </summary>
    public interface IValidationResult
    {
        /// <summary>
        /// Indicates whether the validation was successful or not.
        /// </summary>
        bool IsValid { get; }
        /// <summary>
        /// A more descriptive message of why the validation failed. If <see cref="IsValid"/> is true, this may be null or an empty string.
        /// </summary>
        string Message { get; }
    }
}