using System.Windows.Input;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;
using ChamiUI.Taskbar.Commands;

namespace ChamiUI.Taskbar
{
    /// <summary>
    /// Viewmodel for the taskbar icon
    /// </summary>
    public class TaskbarBehaviourViewModel : ViewModelBase
    {
        /// <summary>
        /// Command that, when triggered, shows the main window.
        /// </summary>
        public ICommand ShowWindowCommand
        {
            get => new ShowWindowCommand();
        }

        /// <summary>
        /// Command that, when triggered, hides the main window.
        /// </summary>
        public ICommand HideWindowCommand => new HideWindowCommand();
        
        /// <summary>
        /// Command to close the application.
        /// </summary>
        public ICommand ExitApplicationCommand => new ExitApplicationCommand();

        private string _currentEnvironmentName;

        public void OnEnvironmentChanged(object sender, EnvironmentChangedEventArgs args)
        {
            if (args?.NewActiveEnvironment?.Name != null)
            {
                _currentEnvironmentName = args.NewActiveEnvironment.Name;
                OnPropertyChanged(nameof(TooltipText));
                return;
            }

            _currentEnvironmentName = null;
        }

        /// <summary>
        /// The text to show in the tooltip.
        /// </summary>
        public string TooltipText
        {
            get
            {
                if (_currentEnvironmentName == null)
                {
                    return "Chami - No environment active";
                }

                return $"Chami - {_currentEnvironmentName}";
            }
        }
    }
}