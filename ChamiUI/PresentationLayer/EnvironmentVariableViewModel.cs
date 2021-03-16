using System;

namespace ChamiUI.PresentationLayer
{
    public class EnvironmentVariableViewModel : ViewModelBase
    {
        private string _name;
        private string _value;
        private DateTime _addedOn;
        private EnvironmentViewModel _environment;

        public EnvironmentViewModel Environment
        {
            get
            {
                return _environment;
            }
            set
            {
                _environment = value;
                OnPropertyChanged(nameof(Environment));
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

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }
    }
}