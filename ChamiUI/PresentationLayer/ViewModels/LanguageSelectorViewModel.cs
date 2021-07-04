using System.Collections.ObjectModel;
using ChamiUI.BusinessLayer.Annotations;
using ChamiUI.Controls;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// Viewmodel for the <see cref="LanguageSelectorControl"/>
    /// </summary>
    public class LanguageSelectorViewModel : SettingCategoryViewModelBase
    {
        /// <summary>
        /// Constructs a new <see cref="LanguageSelectorViewModel"/>
        /// </summary>
        public LanguageSelectorViewModel()
        {
            AvailableLanguages = new ObservableCollection<ApplicationLanguageViewModel>();
        }

        /// <summary>
        /// Contains all the languages the Chami UI is available in.
        /// </summary>
        [NonPersistentSetting]
        public ObservableCollection<ApplicationLanguageViewModel> AvailableLanguages { get; set; }

        private ApplicationLanguageViewModel _currentLanguage;

        /// <summary>
        /// The language the Chami UI is displayed in.
        /// </summary>
        public ApplicationLanguageViewModel CurrentLanguage
        {
            get { return _currentLanguage; }
            set
            {
                _currentLanguage = value;
                OnPropertyChanged(nameof(CurrentLanguage));
            }
        }
    }
}