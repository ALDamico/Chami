using System;
using Chami.Plugins.Contracts.ViewModels;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class EnvironmentVariableBlacklistViewModel : ViewModelBase
    {
        private int? _id;

        public int? Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _initialValue;

        public string InitialValue
        {
            get => _initialValue;
            set
            {
                _initialValue = value;
                OnPropertyChanged(nameof(InitialValue));
            }
        }

        private bool _isWindowsDefault;

        public bool IsWindowsDefault
        {
            get => _isWindowsDefault;
            set
            {
                _isWindowsDefault = value;
                OnPropertyChanged(nameof(IsWindowsDefault));
            }
        }

        private bool _isEnabled;

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
            }
        }

        private DateTime? _addedOn;

        public DateTime? AddedOn
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