using System.Windows.Data;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Validators
{
    /// <summary>
    /// Helper class for functions related to validation.
    /// </summary>
    public static class ValidationUtils
    {
        /// <summary>
        /// Converts an <see cref="object"/> to an <see cref="EnvironmentVariableViewModel"/> that the validation rules can use.
        /// The function attempts to convert from <see cref="BindingGroup"/> and from <see cref="EnvironmentVariableViewModel"/>.
        /// </summary>
        /// <param name="value">The object to convert from.</param>
        /// <returns></returns>
        public static EnvironmentVariableViewModel ConvertObjectToValidate(object value)
        {
            if (value is BindingGroup bindingGroup && bindingGroup.Items.Count > 0)
            {
                return (bindingGroup.Items[0] as EnvironmentVariableViewModel);       
            }

            if (value is EnvironmentVariableViewModel viewModel)
            {
                return viewModel;
            }

            return null;
        }
    }
}