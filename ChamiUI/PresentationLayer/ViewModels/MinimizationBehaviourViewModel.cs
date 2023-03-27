using System.Collections.ObjectModel;
using ChamiUI.BusinessLayer.Annotations;
using ChamiUI.BusinessLayer.Services;
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
        public MinimizationBehaviourViewModel(MinimizationService minimizationService)
        {
            _minimizationService = minimizationService;
        }

        private readonly MinimizationService _minimizationService;

        /// <summary>
        /// The action to perform when minimizing the window.
        /// </summary>
        public IMinimizationStrategy MinimizationStrategy
        {
            get => _minimizationService.MinimizationStrategy;
            set
            {
                _minimizationService.MinimizationStrategy = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The list of available <see cref="IMinimizationStrategy"/>.
        /// </summary>
        [NonPersistentSetting]
        public ObservableCollection<IMinimizationStrategy> AvailableStrategies => _minimizationService.AvailableStrategies;
    }
}