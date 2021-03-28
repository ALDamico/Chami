using ChamiUI.BusinessLayer.Adapters;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class NewEnvironmentViewModel : ViewModelBase
    {
        public NewEnvironmentViewModel()
        {
            Environment = new EnvironmentViewModel();
            _dataAdapter = new EnvironmentDataAdapter(App.GetConnectionString());
            HasBeenChanged = false;
        }

        private EnvironmentDataAdapter _dataAdapter;
        private EnvironmentViewModel _environment;

        public bool SaveEnvironment()
        {
            return _dataAdapter.InsertEnvironment(Environment);
        }

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