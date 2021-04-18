namespace ChamiUI.PresentationLayer.ViewModels
{
    public class EnvironmentExportWindowViewModel: ViewModelBase
    {
        private EnvironmentViewModel _environment;

        public EnvironmentViewModel Environment
        {
            get => _environment;
            set
            {
                _environment = value;
                OnPropertyChanged(nameof(Environment));
                OnPropertyChanged((nameof(NumVariables)));
                OnPropertyChanged(nameof(DisplayedName));
            }
        }

        public int NumVariables
        {
            get => Environment.EnvironmentVariables.Count;
        }

        public string DisplayedName
        {
            get
            {
                var environmentName = Environment.Name;
                var numVariables = NumVariables;
                return $"{environmentName} ({numVariables} variables)";
            }
        }
    }
}