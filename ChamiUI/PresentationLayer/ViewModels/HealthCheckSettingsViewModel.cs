namespace ChamiUI.PresentationLayer.ViewModels
{
    public class HealthCheckSettingsViewModel : ViewModelBase
    {
        private double _milliseconds;
        private bool _isEnabled;

        public double Milliseconds
        {
            get => _milliseconds;
            set
            {
                _milliseconds = value;
                OnPropertyChanged(nameof(Milliseconds));
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
            }
        }
    }
}