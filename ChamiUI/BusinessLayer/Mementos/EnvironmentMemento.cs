using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Mementos
{
    public class EnvironmentMemento : IMemento<EnvironmentViewModel>
    {
        private readonly EnvironmentViewModel _state;
        
        public EnvironmentMemento(EnvironmentViewModel state)
        {
            _state = state;
        }
        public EnvironmentViewModel State
        {
            get => _state;
        }
    }
}