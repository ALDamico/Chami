﻿using ChamiUI.BusinessLayer.Annotations;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// Marker class that gives the default <see cref="ExplicitSaveOnlyAttribute"/> value (false)
    /// </summary>
    [ExplicitSaveOnly(false)]
    public abstract class SettingCategoryViewModelBase : ViewModelBase
    {
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }


        public string IconPath
        {
            get => _iconPath;
            set
            {
                _iconPath = value;
                OnPropertyChanged(nameof(IconPath));
            }
        }

        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        private string _displayName;
        private string _description;
        private string _iconPath;
    }
}