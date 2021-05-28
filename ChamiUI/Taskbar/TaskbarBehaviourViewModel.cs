using System.Threading.Tasks;
using System.Windows.Input;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;
using ChamiUI.Taskbar.Commands;

namespace ChamiUI.Taskbar
{
    public class TaskbarBehaviourViewModel :ViewModelBase
    {
        public ICommand ShowWindowCommand
        {
            get => new ShowWindowCommand();
        }

        public ICommand HideWindowCommand => new HideWindowCommand();
        public ICommand ExitApplicationCommand => new ExitApplicationCommand();

        private string _currentEnvironmentName;

        public virtual void OnEnvironmentChanged(object sender, EnvironmentChangedEventArgs args)
        {
            if (args.NewActiveEnvironment != null)
            {
                if (args.NewActiveEnvironment.Name != null)
                {
                    _currentEnvironmentName = args.NewActiveEnvironment.Name;
                    OnPropertyChanged(nameof(TooltipText));
                    return;
                }
            }

            _currentEnvironmentName = null;
        }

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