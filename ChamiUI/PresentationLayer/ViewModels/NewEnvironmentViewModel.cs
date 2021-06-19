using System.Windows.Data;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.BusinessLayer.Validators;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class NewEnvironmentViewModel : NewEnvironmentViewModelBase
    {
        public NewEnvironmentViewModel():base()
        {
            Environment = new EnvironmentViewModel();
        }

        

        
        private EnvironmentViewModel _environment;

        public EnvironmentViewModel GetInsertedEnvironment()
        {
            Environment = DataAdapter.GetEnvironmentByName(EnvironmentName);
            return Environment;
        }

        public EnvironmentViewModel SaveEnvironment()
        {
            return DataAdapter.InsertEnvironment(Environment);
        }

        public override bool IsSaveButtonEnabled
        {
            get
            {
                var validationResult = Validator.Validate(Environment);
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