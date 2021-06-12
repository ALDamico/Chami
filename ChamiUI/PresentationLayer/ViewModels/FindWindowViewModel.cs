using NetOffice.VBIDEApi;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class FindWindowViewModel: ViewModelBase
    {
        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }
    }
}