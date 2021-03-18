namespace ChamiUI.PresentationLayer
{
    public class NewEnvironmentViewModel : ViewModelBase
    {
        public NewEnvironmentViewModel()
        {
            Environment = new EnvironmentViewModel();
        }
        private EnvironmentViewModel _environment;

        public EnvironmentViewModel Environment
        {
            get => _environment;
            set
            {
                _environment = value;
                OnPropertyChanged(nameof(Environment));
            }
        }

        public bool DetectChanges()
        {
            if (Environment.HasBeenChanged)
            {
                return true;
            }

            foreach (var environmentVariable in Environment.EnvironmentVariables)
            {
                if (environmentVariable.HasBeenChanged)
                {
                    return true;
                }
            }

            return false;
        }
    }
}