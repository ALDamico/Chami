namespace ChamiUI.PresentationLayer.ViewModels
{
    public class MainWindowSavedBehaviourViewModel:SettingCategoryViewModelBase
    {
        private bool _isCaseSensitiveSearch;

        public bool IsCaseSensitiveSearch
        {
            get => _isCaseSensitiveSearch;
            set
            {
                _isCaseSensitiveSearch = value;
                OnPropertyChanged(nameof(IsCaseSensitiveSearch));
            }
        }

        public override bool IsExplicitSaveOnly => true;
    }
}