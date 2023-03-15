using System.Collections.ObjectModel;
using System.Linq;
using ChamiUI.BusinessLayer.Mementos;
using ChamiUI.BusinessLayer.Services;
using ChamiUI.Windows.NewEnvironmentWindow;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// Viewmodel for the new environment window
    /// </summary>
    /// <seealso cref="NewEnvironmentWindow"/>
    public sealed class NewEnvironmentViewModel : NewEnvironmentViewModelBase
    {
        /// <summary>
        /// Constructs a new <see cref="NewEnvironmentViewModel"/>
        /// </summary>
        public NewEnvironmentViewModel(NewEnvironmentService environmentService) : base(environmentService)
        {
            Environment = new EnvironmentViewModel();
            _caretaker = new EnvironmentCaretaker();
            CurrentTemplate = TemplateEnvironments.FirstOrDefault();
        }

        public ObservableCollection<EnvironmentViewModel> TemplateEnvironments => _newEnvironmentService.TemplateEnvironments;

        /// <summary>
        /// Determines if the save button is enabled.
        /// </summary>
        public override bool IsSaveButtonEnabled
        {
            get
            {
                var validationResult = Validator.Validate(Environment);
                return validationResult.IsValid;
            }
        }

        private EnvironmentViewModel _currentTemplate;
        private EnvironmentViewModel _previousTemplate;

        public EnvironmentViewModel CurrentTemplate
        {
            get => _currentTemplate;
            set
            {
                _previousTemplate = _currentTemplate;
                _currentTemplate = value;
                ChangeTemplate();
                OnPropertyChanged();
            }
        }

        private readonly EnvironmentCaretaker _caretaker;

        public void ChangeTemplate()
        {
            _caretaker.SaveState(_previousTemplate?.Name, Environment);
            var state = _caretaker.ResumeState(_currentTemplate.Name);
            if (state == null)
            {
                var environment = new EnvironmentViewModel();
                environment.Name = EnvironmentName;
                foreach (var variable in Environment.EnvironmentVariables)
                {
                    environment.EnvironmentVariables.Add(variable);
                }
                foreach (var environmentVariable in CurrentTemplate.EnvironmentVariables)
                {
                    var variable = environment.EnvironmentVariables.FirstOrDefault(v => v.Name == environmentVariable.Name);
                    if (variable != null)
                    {
                        variable.Value = environmentVariable.Value;
                    }
                    else
                    {
                        environment.EnvironmentVariables.Add(new EnvironmentVariableViewModel(){Name = environmentVariable.Name, Value = environmentVariable.Value, Environment = Environment});
                    }
                    
                }

                Environment = environment;
            }
            else
            {
                Environment = state;
            }
        }
    }
}