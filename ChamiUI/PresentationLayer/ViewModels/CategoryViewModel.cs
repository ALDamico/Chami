namespace ChamiUI.PresentationLayer.ViewModels;

public class CategoryViewModel : GenericLabelViewModel
{
    private int? _id;
    private string _description;

    public int? Id
    {
        get => _id;
        set
        {
            _id = value;
            OnPropertyChanged();
        }
    }
    
    public string Description { get; set; }

}