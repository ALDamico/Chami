using System.Windows.Data;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.BusinessLayer.Validators;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class NewEnvironmentViewModel : ViewModelBase
    {
        public NewEnvironmentViewModel()
        {
            Environment = new EnvironmentViewModel();
            _dataAdapter = new EnvironmentDataAdapter(App.GetConnectionString());
            _validator = new EnvironmentViewModelValidator();
        
        }

        private EnvironmentViewModelValidator _validator;

        private EnvironmentDataAdapter _dataAdapter;
        private EnvironmentViewModel _environment;

        public EnvironmentViewModel GetInsertedEnvironment()
        {
            Environment = _dataAdapter.GetEnvironmentByName(EnvironmentName);
            return Environment;
        }

        public EnvironmentViewModel SaveEnvironment()
        {
            return _dataAdapter.InsertEnvironment(Environment);
        }

        public bool IsSaveButtonEnabled
        {
            get
            {
                var validationResult = _validator.Validate(Environment);
                return validationResult.IsValid;
            }
        }


        public string EnvironmentName
        {
            get => Environment.Name;
            set
            {
                Environment.Name = value;
                OnPropertyChanged(nameof(EnvironmentName));
                OnPropertyChanged(nameof(IsSaveButtonEnabled));
            }
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
    }
}