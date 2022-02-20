namespace ChamiUI.PresentationLayer.ViewModels
{
    public class MassUpdateStrategyViewModel : ViewModelBase
    {
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }
}