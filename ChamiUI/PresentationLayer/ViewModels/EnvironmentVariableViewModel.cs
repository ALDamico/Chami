using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;
using Chami.Db.Entities;
using ChamiUI.BusinessLayer.Validators;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// Viewmodel that represents a <see cref="EnvironmentVariable"/> in the Chami application.
    /// </summary>
    public class EnvironmentVariableViewModel : AbstractValidatableViewModel, ICloneable<EnvironmentVariableViewModel>
    {
        static EnvironmentVariableViewModel()
        {
            AddValidationRule(typeof(EnvironmentVariableViewModel).GetProperty(nameof(Name)), new EnvironmentVariableNameValidCharactersValidationRule());
            AddValidationRule(typeof(EnvironmentVariableViewModel).GetProperty(nameof(Name)), new EnvironmentVariableNameLengthValidationRule(){MaxLength = 2047});
            AddValidationRule(typeof(EnvironmentVariableViewModel).GetProperty(nameof(Name)), new EnvironmentVariableNameNoNumberFirstCharacterValidationRule());
        }
        public EnvironmentVariableViewModel()
        {
            _markedForDeletion = false;
            Value = "";
            
           
        }

        private string _name;
        private string _value;
        private DateTime _addedOn;
        private bool? _isValid;
        private bool _markedForDeletion;
        private bool _isFolder;
        private bool _isDuplicate;

        public bool IsDuplicate
        {
            get => _isDuplicate;
            set
            {
                _isDuplicate = value;
                OnPropertyChanged();
            }
        }

        public bool IsFolder
        {
            get => _isFolder;
            set
            {
                _isFolder = value;
                OnPropertyChanged(nameof(IsFolder));
            }
        }

        public bool MarkedForDeletion
        {
            get => _markedForDeletion;
            set
            {
                _markedForDeletion = value;
                OnPropertyChanged(nameof(MarkedForDeletion));
            }
        }


        public void MarkForDeletion()
        {
            _markedForDeletion = true;
            OnPropertyChanged(nameof(MarkedForDeletion));
        }

        /// <summary>
        /// The result of a <see cref="ValidationRule"/> that has been run on this variable.
        /// </summary>
        public bool? IsValid
        {
            get => _isValid;
            set
            {
                _isValid = value;
                OnPropertyChanged(nameof(_isValid));
            }
        }

        /// <summary>
        /// The Id used by the database.
        /// </summary>
        public int Id { get; set; }

        private EnvironmentViewModel _environment;

        /// <summary>
        /// The environment this variable belongs to.
        /// </summary>
        public EnvironmentViewModel Environment
        {
            get => _environment;
            set
            {
                _environment = value;
                OnPropertyChanged(nameof(Environment));
            }
        }

        /// <summary>
        /// The date this variable was saved to the datastore.
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

        /// <summary>
        /// The name of the variable.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        /// <summary>
        /// The value of the variable.
        /// </summary>
        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public EnvironmentVariableViewModel Clone()
        {
            var clone = new EnvironmentVariableViewModel()
            {
                IsFolder = IsFolder,
                IsValid = IsValid, 
                MarkedForDeletion = false, 
                Name = Name, 
                Value = Value
            };
            return clone;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}