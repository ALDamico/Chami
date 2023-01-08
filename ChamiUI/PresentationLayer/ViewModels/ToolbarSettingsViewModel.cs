using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ChamiUI.PresentationLayer.ViewModels;

public class ToolbarSettingsViewModel : GenericLabelViewModel
{
    public ObservableCollection<ToolBarViewModel> ToolBarViewModels { get; } =
        new ObservableCollection<ToolBarViewModel>();
}