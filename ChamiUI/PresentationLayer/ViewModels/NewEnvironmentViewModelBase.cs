using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.BusinessLayer.Validators;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// Base viewmodel class used by the new environment and the import environment windows.
    /// </summary>
    public abstract class NewEnvironmentViewModelBase : ViewModelBase
    {
        /// <summary>
        /// Constructs a new <see cref="NewEnvironmentViewModelBase"/> object and initializes its <see cref="DataAdapter"/>
        /// </summary>
        protected NewEnvironmentViewModelBase(EnvironmentDataAdapter environmentDataAdapter) : this()
        {
            DataAdapter = environmentDataAdapter;
        }

        protected NewEnvironmentViewModelBase()
        {
            Validator = new EnvironmentViewModelValidator();
        }

        /// <summary>
        /// Performs validation on the environment before saving it.
        /// </summary>
        protected EnvironmentViewModelValidator Validator { get; }

        /// <summary>
        /// Allows saving the new environment.
        /// </summary>
        protected EnvironmentDataAdapter DataAdapter { get; }

        /// <summary>
        /// Determines if the save button is enabled.
        /// </summary>
        public abstract bool IsSaveButtonEnabled { get; }
    }
}