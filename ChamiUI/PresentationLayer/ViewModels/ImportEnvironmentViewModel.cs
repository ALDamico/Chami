using System.Collections.ObjectModel;
using ChamiUI.PresentationLayer.Events;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class ImportEnvironmentViewModel : EnvironmentViewModel
    {
        public ImportEnvironmentViewModel()
        {
            Messages = new ObservableCollection<GenericInfoViewModel>();
        }

        private bool _shouldImport;
        private bool _exists;

        public bool Exists
        {
            get => _exists;
            set
            {
                _exists = value;
                OnPropertyChanged();
            }
        }

        public bool ShouldImport
        {
            get => _shouldImport;
            set
            {
                _shouldImport = value;
                OnPropertyChanged(nameof(ShouldImport));
            }
        }

        public ObservableCollection<GenericInfoViewModel> Messages { get; }
        public override string Name
        {
            get => base.Name;
            set
            {
                base.Name = value;
                
            }
        }
    }
}