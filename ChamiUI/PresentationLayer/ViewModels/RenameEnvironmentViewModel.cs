using System.Windows.Input;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// Viewmodel used by the rename environment window.
    /// </summary>
    public class RenameEnvironmentViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes the commands of this viewmodel.
        /// </summary>
        static RenameEnvironmentViewModel()
        {
            RenameEnvironmentCommand = new RoutedCommand();
            CancelRenamingCommand = new RoutedCommand();
        }
        
        /// <summary>
        /// Default constructor that does nothing.
        /// </summary>
        public RenameEnvironmentViewModel()
        {
            
        }

        /// <summary>
        /// Constructs a new <see cref="RenameEnvironmentViewModel"/> object and sets its <see cref="Name"/> property.
        /// </summary>
        /// <param name="initialName"></param>
        public RenameEnvironmentViewModel(string initialName)
        {
            Name = initialName;
        }
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
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(IsNameValid));
            }
        }
        
        /// <summary>
        /// True if the name is valid, otherwise false.
        /// </summary>
        public bool IsNameValid => !string.IsNullOrWhiteSpace(Name);
        public static readonly RoutedCommand RenameEnvironmentCommand;
        public static readonly RoutedCommand CancelRenamingCommand;

    }
}