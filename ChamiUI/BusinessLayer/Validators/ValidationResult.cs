namespace ChamiUI.BusinessLayer.Validators
{
    public class ValidationResult : IValidationResult
    {
        public ValidationResult()
        {

        }

        public ValidationResult(bool isValid, string message)
        {
            IsValid = isValid;
            Message = message;
        }
        public bool IsValid { get; set; }
        public string Message { get; set; }
    }
}