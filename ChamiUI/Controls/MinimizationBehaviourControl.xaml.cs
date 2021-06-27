using System.Windows.Controls;
using ChamiUI.PresentationLayer.Minimizing;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Controls
{
    public partial class MinimizationBehaviourControl : UserControl
    {
        public MinimizationBehaviourControl(MinimizationBehaviourViewModel minimizationBehaviourViewModel)
        {
            var viewModel = minimizationBehaviourViewModel;
            DataContext = viewModel;
            InitializeComponent();
        }
/*
        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as MinimizationBehaviourViewModel).MinimizationStrategy = (IMinimizationStrategy)e.AddedItems[0];
        }*/
    }
}