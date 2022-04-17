using System.Collections.ObjectModel;
using ChamiUI.BusinessLayer.Annotations;
using ChamiUI.PresentationLayer.Minimizing;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// Represents the settings that control how the main window behaves when it's minimized.
    /// </summary>
    public class MinimizationBehaviourViewModel : GenericLabelViewModel
    {
        /// <summary>
        /// Constructs a new <see cref="MinimizationBehaviourViewModel"/> and populates its list of available
        /// strategies.
        /// </summary>
        public MinimizationBehaviourViewModel()
        {
            AvailableStrategies = new ObservableCollection<IMinimizationStrategy>();
            AvailableStrategies.Add(MinimizeToTaskbarStrategy.Instance);
            AvailableStrategies.Add(MinimizeToTrayStrategy.Instance);
        }

        private IMinimizationStrategy _minimizationStrategy;

        /// <summary>
        /// The action to perform when minimizing the window.
        /// </summary>
        public IMinimizationStrategy MinimizationStrategy
        {
            get => _minimizationStrategy;
            set
            {
                _minimizationStrategy = value;
                OnPropertyChanged(nameof(MinimizationStrategy));
            }
        }

        /// <summary>
        /// The list of available <see cref="IMinimizationStrategy"/>.
        /// </summary>
        [NonPersistentSetting]
        public ObservableCollection<IMinimizationStrategy> AvailableStrategies { get; }
    }
}