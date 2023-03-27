using System.Collections.ObjectModel;
using ChamiUI.PresentationLayer.Minimizing;

namespace ChamiUI.BusinessLayer.Services;

public class MinimizationService
{
    public MinimizationService()
    {
        AvailableStrategies = new ObservableCollection<IMinimizationStrategy>();
    }
    public ObservableCollection<IMinimizationStrategy> AvailableStrategies { get; }
    public IMinimizationStrategy MinimizationStrategy { get; set; }
}