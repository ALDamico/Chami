using System.Collections.Generic;

namespace ChamiUI.PresentationLayer.ViewModels;

public class ToolBarViewModel:ViewModelBase
{
    private string _name;
    private bool _isVisible;
    public List<ToolbarButtonViewModel> ToolbarButtonViewModels { get; } = new List<ToolbarButtonViewModel>();

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            _isVisible = value;
            OnPropertyChanged();
        }
    }
}