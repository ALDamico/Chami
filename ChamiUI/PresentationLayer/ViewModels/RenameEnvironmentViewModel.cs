using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using ChamiUI.BusinessLayer.Services;
using ChamiUI.Localization;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// Viewmodel used by the rename environment window.
    /// </summary>
    public class RenameEnvironmentViewModel : ViewModelBase
    {
        /// <summary>
        /// Constructs a new <see cref="RenameEnvironmentViewModel"/> object
        /// </summary>
        /// <param name="renameEnvironmentService">The underlying service to this viewmodel.</param>
        public RenameEnvironmentViewModel(RenameEnvironmentService renameEnvironmentService) : base()
        {
            _renameEnvironmentService = renameEnvironmentService;
            RenameCommand = new AsyncCommand<Window>(ExecuteRename, CanExecuteRename);
        }

        private bool CanExecuteRename(object arg)
        {
            return IsNameValid;
        }

        private async Task ExecuteRename(Window arg)
        {
            await _renameEnvironmentService.RenameEnvironment(Name);
            await CloseCommand.ExecuteAsync(arg);
        }

        private readonly RenameEnvironmentService _renameEnvironmentService;
        private string _name;

        /// <summary>
        /// The new name to apply to the environment.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNameValid));
                OnPropertyChanged(nameof(NameInvalidToolTip));
                RenameCommand.RaiseCanExecuteChanged();
            }
        }

        public string NameInvalidToolTip
        {
            get
            {
                if (!IsNameValid)
                {
                    return ChamiUIStrings.NameInvalidToolTip;
                }

                return null;
            }
        }
        
        /// <summary>
        /// True if the name is valid, otherwise false.
        /// </summary>
        public bool IsNameValid => !string.IsNullOrWhiteSpace(Name);
        public IAsyncCommand<Window> RenameCommand { get; }

    }
}