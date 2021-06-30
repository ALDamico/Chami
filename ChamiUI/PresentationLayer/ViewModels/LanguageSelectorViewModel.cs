using System.Collections.ObjectModel;
using ChamiUI.BusinessLayer.Annotations;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class LanguageSelectorViewModel:SettingCategoryViewModelBase
    {
        public LanguageSelectorViewModel()
        {
            AvailableLanguages = new ObservableCollection<ApplicationLanguageViewModel>();
        }
        [NonPersistentSetting]
        public ObservableCollection<ApplicationLanguageViewModel> AvailableLanguages { get; set;   }
        private ApplicationLanguageViewModel _currentLanguage;

        public ApplicationLanguageViewModel CurrentLanguage
        {
            get
            {
                return _currentLanguage;
            }
            set
            {
                _currentLanguage = value;
                OnPropertyChanged(nameof(CurrentLanguage));
            }
        }
    }
}