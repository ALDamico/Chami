using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Validators
{
    public class EnvironmentVariableHealthStatus
    {
        public EnvironmentVariableViewModel EnvironmentVariable { get; set; }
        public string ActualValue { get; set; }
        public string ExpectedValue { get; set; }
        public EnvironmentVariableHealthType IssueType { get; set; }
    }
}