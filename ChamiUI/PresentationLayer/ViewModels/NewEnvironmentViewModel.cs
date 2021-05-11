using ChamiUI.BusinessLayer.Adapters;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class NewEnvironmentViewModel : ViewModelBase
    {
        public NewEnvironmentViewModel()
        {
            Environment = new EnvironmentViewModel();
            _dataAdapter = new EnvironmentDataAdapter(App.GetConnectionString());
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
            return true;
        }
    }
}