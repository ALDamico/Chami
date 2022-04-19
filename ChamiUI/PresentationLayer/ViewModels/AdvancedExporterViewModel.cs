using ChamiUI.BusinessLayer.Exporters;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class AdvancedExporterViewModel : ViewModelBase
    {
        private string _name;
        private IChamiExporter _backingExporter;

        public IChamiExporter BackingExporter
        {
            get => _backingExporter;
            set
            {
                _backingExporter = value;
                OnPropertyChanged(nameof(BackingExporter));
            }
        }

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