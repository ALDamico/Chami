using Serilog.Events;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class LogLevelViewModel : GenericLabelViewModel
    {
        private LogEventLevel _backingValue;
        
        public LogEventLevel BackingValue
        {
            get => _backingValue;
            set
            {
                _backingValue = value;
                OnPropertyChanged(nameof(BackingValue));
            }
        }
    }
}