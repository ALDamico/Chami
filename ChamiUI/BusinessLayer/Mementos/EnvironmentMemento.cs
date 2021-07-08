using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Mementos
{
    public class EnvironmentMemento
    {
        public EnvironmentMemento(EnvironmentViewModel environment)
        {
            Environment = environment;
        }
        public EnvironmentViewModel Environment { get; }
    }
}