using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Mementos
{
    public class EnvironmentOriginator
    {
        private EnvironmentViewModel _state;
        public EnvironmentViewModel State => _state;

        public void SetState(EnvironmentViewModel state)
        {
            _state = state;
        }

        public IMemento<EnvironmentViewModel> CreateMemento()
        {
            return new EnvironmentMemento(_state);
        }

        public void RestoreState(IMemento<EnvironmentViewModel> memento)
        {
            _state = memento.State;
        }
    }
}