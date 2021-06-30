using System.Collections.ObjectModel;
using ChamiUI.BusinessLayer.Annotations;
using ChamiUI.PresentationLayer.Minimizing;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class MinimizationBehaviourViewModel : ViewModelBase
    {
        public MinimizationBehaviourViewModel()
        {
            AvailableStrategies = new ObservableCollection<IMinimizationStrategy>();
            AvailableStrategies.Add( MinimizeToTaskbarStrategy.Instance);
            AvailableStrategies.Add( MinimizeToTrayStrategy.Instance);
        }

        private IMinimizationStrategy _minimizationStrategy;

        public IMinimizationStrategy MinimizationStrategy
        {
            get => _minimizationStrategy; 
            set
            {
                _minimizationStrategy = value;
                OnPropertyChanged(nameof(MinimizationStrategy));
            }
        }

        [NonPersistentSetting]
        public ObservableCollection<IMinimizationStrategy> AvailableStrategies { get; }
    }
}