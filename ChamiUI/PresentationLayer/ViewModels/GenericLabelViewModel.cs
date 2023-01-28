using System.Windows.Media;
using ChamiUI.BusinessLayer.Annotations;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// Marker class that gives the default <see cref="ExplicitSaveOnlyAttribute"/> value (false)
    /// </summary>
    [ExplicitSaveOnly(false)]
    public abstract class GenericLabelViewModel : ViewModelBase
    {
        protected GenericLabelViewModel()
        {
            IsVisible = true;
        }
        [NonPersistentSetting]
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        [NonPersistentSetting]
        public string IconPath
        {
            get => _iconPath;
            set
            {
                _iconPath = value;
                OnPropertyChanged(nameof(IconPath));
            }
        }

        [NonPersistentSetting]
        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        [NonPersistentSetting]
        public Brush ForegroundColor
        {
            get => _foregroundColor;
            set
            {
                _foregroundColor = value;
                OnPropertyChanged(nameof(ForegroundColor));
            }
        }

        [NonPersistentSetting]
        public Brush BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;
                OnPropertyChanged();
            }
        }

        [NonPersistentSetting]
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        private string _displayName;
        private string _description;
        private string _iconPath;
        private Brush _foregroundColor;
        private Brush _backgroundColor;
        private bool _isVisible;
    }
}