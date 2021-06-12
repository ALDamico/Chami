using System.Windows.Data;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Validators
{
    public static class ValidationUtils
    {
        public static EnvironmentVariableViewModel ConvertObjectToValidate(object value)
        {
            if (value is BindingGroup bindingGroup)
            {
                if (bindingGroup.Items.Count > 0)
                {
                    return (bindingGroup.Items[0] as EnvironmentVariableViewModel);                    
                }
                
            }

            if (value is EnvironmentVariableViewModel viewModel)
            {
                return viewModel;
            }

            return null;
        }
    }
}