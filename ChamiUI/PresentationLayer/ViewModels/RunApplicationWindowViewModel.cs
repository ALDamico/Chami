using System.Collections.ObjectModel;

namespace ChamiUI.PresentationLayer.ViewModels;

public class RunApplicationWindowViewModel : ViewModelBase
{
    public RunApplicationWindowViewModel()
    {
        WatchedApplications = new ObservableCollection<WatchedApplicationViewModel>();
    }
    public ObservableCollection<WatchedApplicationViewModel> WatchedApplications { get; }
}