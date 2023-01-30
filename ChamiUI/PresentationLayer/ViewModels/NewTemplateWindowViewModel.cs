using Chami.Db.Entities;
using ChamiUI.BusinessLayer.Adapters;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class NewTemplateWindowViewModel : NewEnvironmentViewModelBase
    {
        public NewTemplateWindowViewModel(EnvironmentDataAdapter environmentDataAdapter) : base(environmentDataAdapter)
        {
            Environment = new EnvironmentViewModel() {EnvironmentType = EnvironmentType.TemplateEnvironment};
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

        public string TemplateName
        {
            get => Environment.Name;
            set
            {
                Environment.Name = value;
                OnPropertyChanged(nameof(TemplateName));
                OnPropertyChanged(nameof(Environment));
                OnPropertyChanged(nameof(IsSaveButtonEnabled));
            }
        }

        public void SaveTemplate()
        {
            DataAdapter.SaveTemplateEnvironment(Environment);
        }


        public override bool IsSaveButtonEnabled
        {
            get
            {
                if (string.IsNullOrWhiteSpace(TemplateName))
                {
                    return false;
                }

                var validationResult = Validator.Validate(Environment);
                if (validationResult.IsValid)
                {
                    return true;
                }

                return false;
            }
        }
    }
}