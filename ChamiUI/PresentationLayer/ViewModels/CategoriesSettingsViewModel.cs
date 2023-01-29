using System.Collections.ObjectModel;

namespace ChamiUI.PresentationLayer.ViewModels;

public class CategoriesSettingsViewModel : GenericLabelViewModel
{
    public ObservableCollection<CategoryViewModel> AvailableCategories { get; set; }
    public CategoryViewModel SelectedCategory { get; set; }
}