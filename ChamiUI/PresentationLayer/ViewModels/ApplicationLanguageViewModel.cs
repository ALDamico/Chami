using Chami.Db.Entities;
using Chami.Plugins.Contracts.ViewModels;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// A viewmodel for <see cref="UiLanguage"/>, as used by the GUI of the Chami application.
    /// </summary>
    public class ApplicationLanguageViewModel : ViewModelBase
    {
        private string _name;

        /// <summary>
        /// Display name for the language.
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

        private string _code;

        /// <summary>
        /// ISO-639 code of the language.
        /// </summary>
        public string Code
        {
            get => _code;
            set
            {
                _code = value;
                OnPropertyChanged(nameof(Code));
            }
        }

        private string _iconPath;

        /// <summary>
        /// Path to the SVG image of the language's flag.
        /// </summary>
        public string IconPath
        {
            get => _iconPath;
            set
            {
                _iconPath = value;
                OnPropertyChanged(nameof(IconPath));
            }
        }

        public override string ToString()
        {
            return Code;
        }
    }
}