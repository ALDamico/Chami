
using Chami.Db.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChamiUI.BusinessLayer.Mementos;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class NewTemplateWindowViewModel : NewEnvironmentViewModelBase
    {
        public EnvironmentCaretaker Caretaker { get; set; }
        public NewTemplateWindowViewModel()
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

        public string TemplateName
        {
            get => Environment.Name;
            set
            {
                Environment.Name = value;
                OnPropertyChanged(nameof(TemplateName));
                OnPropertyChanged(nameof(Environment));
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
