using ChamiUI.Localization;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class MassUpdateStrategyViewModel : ViewModelBase
    {
        private string _name;
        private bool _createIfNotExists;
        private bool _createIfNotExistsEnabled;
        private bool _environmentListBoxEnabled;

        /// <summary>
        /// Determines if the listbox containing the environments is enabled or not.
        /// </summary>
        public bool EnvironmentListBoxEnabled
        {
            get => _environmentListBoxEnabled;
            set
            {
                _environmentListBoxEnabled = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Determines if the user can tick the "Create variable if not existing" checkbox.
        /// </summary>
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

        /// <summary>
        /// If true, the application will create the selected variable(s) if they don't exist in the environments to
        /// update.
        /// </summary>
        public bool CreateIfNotExists
        {
            get => _createIfNotExists;
            set
            {
                _createIfNotExists = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The name of the variable the user wishes to update.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private static MassUpdateStrategyViewModel _defaultUpdateStrategy;

        /// <summary>
        /// The default strategy to use when updating the environments.
        /// </summary>
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