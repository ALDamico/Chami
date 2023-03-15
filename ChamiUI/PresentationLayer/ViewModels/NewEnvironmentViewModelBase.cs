using System.Windows;
using AsyncAwaitBestPractices.MVVM;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.BusinessLayer.Services;
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
        protected NewEnvironmentViewModelBase(NewEnvironmentService newEnvironmentService) : this()
        {
            _newEnvironmentService = newEnvironmentService;
        }

        protected NewEnvironmentService _newEnvironmentService;

        protected NewEnvironmentViewModelBase()
        {
            Validator = new EnvironmentViewModelValidator();
        }

        /// <summary>
        /// Performs validation on the environment before saving it.
        /// </summary>
        protected EnvironmentViewModelValidator Validator { get; }

        /// <summary>
        /// Determines if the save button is enabled.
        /// </summary>
        public abstract bool IsSaveButtonEnabled { get; }
        /// <summary>
        /// The new environment to insert.
        /// </summary>
        public virtual EnvironmentViewModel Environment
        {
            get => _environment;
            set
            {
                _environment = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsSaveButtonEnabled));
                OnPropertyChanged(nameof(EnvironmentName));
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
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsSaveButtonEnabled));
            }
        }
        private EnvironmentViewModel _environment;
        
        public IAsyncCommand<Window> SaveCommand { get; protected set; }
    }
}