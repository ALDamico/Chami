using System.Collections.ObjectModel;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class LanguageSelectorViewModel:ViewModelBase
    {
        public LanguageSelectorViewModel()
        {
            AvailableLanguages = new ObservableCollection<ApplicationLanguageViewModel>();
        }
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