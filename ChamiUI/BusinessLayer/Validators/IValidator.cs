namespace ChamiUI.BusinessLayer.Validators
{
    public interface IValidator<in T>
    {
        IValidationResult Validate(T viewModel);
    }
}