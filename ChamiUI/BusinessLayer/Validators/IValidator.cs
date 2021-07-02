using System;

namespace ChamiUI.BusinessLayer.Validators
{
    /// <summary>
    /// Interface for classes that perform basic validation on objects of type <see cref="T"/>
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of object to validate.</typeparam>
    public interface IValidator<in T>
    {
        /// <summary>
        /// Performs validation on an object.
        /// </summary>
        /// <param name="viewModel">The object to validate.</param>
        /// <returns>A <see cref="IValidationResult"/> object.</returns>
        IValidationResult Validate(T viewModel);
    }
}