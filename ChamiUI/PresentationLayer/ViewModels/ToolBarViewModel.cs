using System.Collections.Generic;

namespace ChamiUI.PresentationLayer.ViewModels;

public class ToolBarViewModel:ViewModelBase
{
    public List<ToolbarButtonViewModel> ToolbarButtonViewModels { get; } = new List<ToolbarButtonViewModel>();
    
    public string Name { get; set; }
    public bool IsVisible { get; set; }
}