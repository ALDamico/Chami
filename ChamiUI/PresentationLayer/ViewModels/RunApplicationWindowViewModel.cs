using System.Collections.ObjectModel;
using ChamiUI.Utils;

namespace ChamiUI.PresentationLayer.ViewModels;

public class RunApplicationWindowViewModel : ViewModelBase
{
    public RunApplicationWindowViewModel()
    {
        
    }

    public ObservableCollection<WatchedApplicationViewModel> WatchedApplications =>
        AppUtils.GetChamiApp().Settings.WatchedApplicationSettings.WatchedApplications;
}