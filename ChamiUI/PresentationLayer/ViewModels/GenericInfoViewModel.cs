using ChamiUI.BusinessLayer.Enums;

namespace ChamiUI.PresentationLayer.ViewModels;

public class GenericInfoViewModel : ViewModelBase
{
    public GenericInfoViewModel(InformationSeverity severity, string message)
    {
        Severity = severity;
        Message = message;
    }

    public GenericInfoViewModel(InformationSeverity severity, string key, string message) : this(severity, message)
    {
        Key = key;
    }
    private InformationSeverity _severity;

    public InformationSeverity Severity
    {
        get => _severity;
        set
        {
            _severity = value;
            OnPropertyChanged();
        }
    }

    private string _message;

    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            OnPropertyChanged();
        }
    }

    private string _key;

    public string Key
    {
        get => _key;
        set
        {
            _key = value;
            OnPropertyChanged();
        }
    }
    
    public string InformationType { get; set; }
}