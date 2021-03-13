using System;
using System.Collections.ObjectModel;

namespace ChamiUI.PresentationLayer
{
    public class EnvironmentViewModel : ViewModelBase
    {
        public EnvironmentViewModel()
        {
            EnvironmentVariables = new ObservableCollection<EnvironmentVariableViewModel>();
        }
        private int _id;
        private DateTime _addedOn;
        private string _name;
        
        public ObservableCollection<EnvironmentVariableViewModel> EnvironmentVariables { get; }

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public DateTime AddedOn
        {
            get => _addedOn;
            set
            {
                _addedOn = value;
                OnPropertyChanged(nameof(AddedOn));
            }
        }
    }
}