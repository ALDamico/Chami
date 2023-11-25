namespace ChamiUI.PresentationLayer.ViewModels;

public class CategoryViewModel : GenericLabelViewModel
{
    private int? _id;

    public int? Id
    {
        get => _id;
        set
        {
            _id = value;
            OnPropertyChanged();
        }
    }
}