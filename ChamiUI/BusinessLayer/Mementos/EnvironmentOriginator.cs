using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Mementos
{
    public class EnvironmentOriginator
    {
        public EnvironmentViewModel State { get; set; }

        public EnvironmentMemento SaveMemento()
        {
            return new EnvironmentMemento(State);
        }

        public void RestoreMemento(EnvironmentMemento memento)
        {
            State = memento.Environment;
        }
    }
}