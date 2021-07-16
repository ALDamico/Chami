using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Controls
{
    public partial class MinimizationBehaviourControl
    {
        /// <summary>
        /// Constructs a new <see cref="MinimizationBehaviourControl"/> and sets its viewmodel.
        /// </summary>
        /// <param name="minimizationBehaviourViewModel">The starting viewmodel.</param>
        public MinimizationBehaviourControl(MinimizationBehaviourViewModel minimizationBehaviourViewModel)
        {
            var viewModel = minimizationBehaviourViewModel;
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}