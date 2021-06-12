using System.Windows.Controls;
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

        private object _foreignDataContext;

        public object ForeignDataContext
        {
            get => _foreignDataContext;
            set
            {
                _foreignDataContext = value;
                OnPropertyChanged(nameof(ForeignDataContext));
            }
        }
    }
}