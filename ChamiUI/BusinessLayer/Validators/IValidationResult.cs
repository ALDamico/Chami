namespace ChamiUI.BusinessLayer.Validators
{
    public interface IValidationResult
    {
        bool IsValid { get; }
        string Message { get; }
    }
}