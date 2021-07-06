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
            }
        }
    }
}