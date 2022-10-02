using System;
using System.Threading.Tasks;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.Localization;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class MassUpdateStrategyViewModel : ViewModelBase
    {
        private string _name;
        private bool _createIfNotExists;
        private bool _createIfNotExistsEnabled;
        private bool _environmentListBoxEnabled;

        public bool EnvironmentListBoxEnabled
        {
            get => _environmentListBoxEnabled;
            set
            {
                _environmentListBoxEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool CreateIfNotExistsEnabled
        {
            get => _createIfNotExistsEnabled;
            set
            {
                _createIfNotExistsEnabled = value;
                OnPropertyChanged(nameof(CreateIfNotExists));
                OnPropertyChanged();
            }
        }

        public bool CreateIfNotExists
        {
            get => _createIfNotExists;
            set
            {
                _createIfNotExists = value;
                OnPropertyChanged();
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

        private static MassUpdateStrategyViewModel _defaultUpdateStrategy;

        public static MassUpdateStrategyViewModel DefaultUpdateStrategy
        {
            get
            {
                if (_defaultUpdateStrategy == null)
                {
                    _defaultUpdateStrategy = new MassUpdateStrategyViewModel()
                    {
                        Name = ChamiUIStrings.MassUpdateStrategyName_UpdateAll, 
                        CreateIfNotExistsEnabled = true,
                        EnvironmentListBoxEnabled = false
                    };
                }

                return _defaultUpdateStrategy;
            }
        }
    }
}