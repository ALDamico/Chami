using System;

namespace ChamiUI.PresentationLayer
{
    public class EnvironmentVariableViewModel:ViewModelBase
    {
        private string _name;
        private string _value;
        private DateTime _addedOn;

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