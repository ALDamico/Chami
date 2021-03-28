using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Validators
{
    public interface IValidator<T> where T : ViewModelBase
    {
        IValidationResult Validate(T viewModel);
    }
}