using System.ComponentModel;
using ChamiUI.PresentationLayer.Filtering;

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
        
        public double Height { get; set; }
        public double Width { get; set; }
        public double XPosition { get; set; }
        public double YPosition { get; set; }
        public IFilterStrategy SearchPath { get; set; }
        public SortDescription SortDescription { get; set; }
    }
}