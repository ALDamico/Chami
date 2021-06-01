using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class EnvironmentViewModel : ViewModelBase
    {
        public EnvironmentViewModel()
        {
            EnvironmentVariables = new ObservableCollection<EnvironmentVariableViewModel>();
            AddedOn = DateTime.Now;
        }

        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                OnPropertyChanged(nameof(IsActive));
                OnPropertyChanged(nameof(TextFontWeight));
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        public FontWeight TextFontWeight
        {
            get
            {
                if (IsActive)
                {
                    return FontWeights.Bold;
                }
                return FontWeights.Regular;
            }
        }

        public string DisplayName
        {
            get
            {
                if (IsActive)
                {
                    return $"{Name} *";
                }

                return Name;
            }
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
                OnPropertyChanged(nameof(DisplayName));
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

        public override bool Equals(object? obj)
        {
            var environmentViewModel2 = obj as EnvironmentViewModel;
            if (environmentViewModel2 == null)
            {
                return false;
            }

            return environmentViewModel2.Id == Id;
        }
    }
}