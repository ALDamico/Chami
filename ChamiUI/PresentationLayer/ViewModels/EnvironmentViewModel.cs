using System;
using System.Collections.ObjectModel;
using System.Windows;
using Chami.Db.Entities;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// Represents an environment (i.e., a collection of environment variables) saved in the datastore and converted
    /// for use by the Chami UI.
    /// </summary>
    public class EnvironmentViewModel : ViewModelBase
    {
        /// <summary>
        /// Constructs a new <see cref="EnvironmentViewModel"/> object and initializes it to its default state.
        /// </summary>
        public EnvironmentViewModel()
        {
            EnvironmentVariables = new ObservableCollection<EnvironmentVariableViewModel>();
            AddedOn = DateTime.Now;
        }

        private bool _isActive;

        /// <summary>
        /// Represents if this viewmodel is the currently active environment.
        /// </summary>
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

        /// <summary>
        /// The <see cref="FontWeight"/> this environment will have in the listvview. The active environment will be in
        /// bold, while others will be in a regular font weight.
        /// </summary>
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

        /// <summary>
        /// The displayed name in the listview. The active environment will have an asterisk after its name, other
        /// environments won't.
        /// </summary>
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

        /// <summary>
        /// The environment variables that belong to this environment.
        /// </summary>
        public ObservableCollection<EnvironmentVariableViewModel> EnvironmentVariables { get; }

        /// <summary>
        /// The id of this environment in the datastore.
        /// </summary>
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        /// <summary>
        /// The name of this environment.
        /// </summary>
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

        /// <summary>
        /// The creation date of this environment.
        /// </summary>
        public DateTime AddedOn
        {
            get => _addedOn;
            set
            {
                _addedOn = value;
                OnPropertyChanged(nameof(AddedOn));
            }
        }

        private EnvironmentType _environmentType;

        public EnvironmentType EnvironmentType
        {
            get => _environmentType;
            set
            {
                _environmentType = value;
                OnPropertyChanged(nameof(EnvironmentType));
            }
        }

        /// <summary>
        /// Compares two environments by their id.
        /// </summary>
        /// <param name="obj">The other environment.</param>
        /// <returns>True if the two ids match, otherwise false.</returns>
        public override bool Equals(object obj)
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