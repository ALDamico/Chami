using ChamiUI.PresentationLayer.Events;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class ImportEnvironmentViewModel : EnvironmentViewModel
    {
        private bool _shouldImport;

        public bool ShouldImport
        {
            get => _shouldImport;
            set
            {
                _shouldImport = value;
                OnPropertyChanged(nameof(ShouldImport));
            }
        }
    }
}