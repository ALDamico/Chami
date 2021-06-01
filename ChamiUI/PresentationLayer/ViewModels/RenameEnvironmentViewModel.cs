using System;
using System.Windows.Input;
using ChamiUI.PresentationLayer.Events;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class RenameEnvironmentViewModel : ViewModelBase
    {
        static RenameEnvironmentViewModel()
        {
            RenameEnvironmentCommand = new RoutedCommand();
            CancelRenamingCommand = new RoutedCommand();
        }
        public RenameEnvironmentViewModel()
        {
            
        }

        public RenameEnvironmentViewModel(string initialName)
        {
            Name = initialName;
        }
        private string _name;

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
        
        public bool IsNameValid => !string.IsNullOrWhiteSpace(Name);
        public static RoutedCommand RenameEnvironmentCommand;
        public static RoutedCommand CancelRenamingCommand;

    }
}