using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using ChamiUI.BusinessLayer.Mementos;
using ChamiUI.BusinessLayer.Validators;
using ChamiUI.Windows.NewEnvironmentWindow;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// Viewmodel for the new environment window
    /// </summary>
    /// <seealso cref="NewEnvironmentWindow"/>
    public class NewEnvironmentViewModel : NewEnvironmentViewModelBase
    {
        /// <summary>
        /// Constructs a new <see cref="NewEnvironmentViewModel"/>
        /// </summary>
        public NewEnvironmentViewModel()
        {
            Environment = new EnvironmentViewModel();
            TemplateEnvironments = new ObservableCollection<EnvironmentViewModel>();
            CurrentTemplate = new EnvironmentViewModel() {Name = "None"};
            TemplateEnvironments.Add(CurrentTemplate);

            var templates = DataAdapter.GetTemplateEnvironments();
            foreach (var template in templates)
            {
                TemplateEnvironments.Add(template);
            }

            _caretaker = new EnvironmentCaretaker();

            ValidationRules.Add(new EnvironmentVariableNameNotNullValidationRule()
                {ValidationStep = ValidationStep.UpdatedValue});
            ValidationRules.Add(new EnvironmentVariableNameValidCharactersValidationRule()
                {ValidationStep = ValidationStep.UpdatedValue});
            ValidationRules.Add(new EnvironmentVariableNameLengthValidationRule()
                {MaxLength = 2047, ValidationStep = ValidationStep.UpdatedValue});
            ValidationRules.Add(new EnvironmentVariableNameNoNumberFirstCharacterValidationRule()
                {ValidationStep = ValidationStep.UpdatedValue});
            var collectionViewSource = new CollectionViewSource();
            collectionViewSource.Source = Environment.EnvironmentVariables;
            ValidationRules.Add(new EnvironmentVariableNameUniqueValidationRule()
                {ValidationStep = ValidationStep.CommittedValue, EnvironmentVariables = collectionViewSource});
        }

        private EnvironmentViewModel _environment;

        /// <summary>
        /// Gets the newly-inserted environment and returns it.
        /// </summary>
        /// <returns>The newly-inserted <see cref="NewEnvironmentViewModel"/>.</returns>
        public EnvironmentViewModel GetInsertedEnvironment()
        {
            Environment = DataAdapter.GetEnvironmentByName(EnvironmentName);
            return Environment;
        }


        /// <summary>
        /// Converts the new <see cref="EnvironmentViewModel"/> to a <see cref="Environment"/> entity and saves it to
        /// the datastore.
        /// </summary>
        /// <returns>The newly-saved environment.</returns>
        public EnvironmentViewModel SaveEnvironment()
        {
            return DataAdapter.InsertEnvironment(Environment);
        }

        public ObservableCollection<EnvironmentViewModel> TemplateEnvironments { get; }

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

        /// <summary>
        /// The name of the new environment.
        /// </summary>
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

        /// <summary>
        /// The new environment to insert.
        /// </summary>
        public EnvironmentViewModel Environment
        {
            get => _environment;
            set
            {
                _environment = value;
                OnPropertyChanged(nameof(Environment));
                OnPropertyChanged(nameof(IsSaveButtonEnabled));
                OnPropertyChanged(nameof(EnvironmentName));
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
                OnPropertyChanged(nameof(CurrentTemplate));
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
                foreach (var environmentVariable in CurrentTemplate.EnvironmentVariables)
                {
                    environment.EnvironmentVariables.Add(new EnvironmentVariableViewModel()
                    {
                        Name = environmentVariable.Name, Value = environmentVariable.Value, Environment = Environment
                    });
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