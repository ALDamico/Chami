namespace ChamiUI.PresentationLayer.ViewModels
{
    public class EnvironmentUpdateViewModel : EnvironmentViewModel
    {
        private bool _shouldUpdate;
        private bool _shouldDelete;
        private bool _shouldAdd;
        private bool _markedForUpdate;

        public bool MarkedForUpdate
        {
            get => _markedForUpdate;
            set
            {
                _markedForUpdate = value;
                OnPropertyChanged(nameof(MarkedForUpdate));
            }
        }

        public bool ShouldAdd
        {
            get => _shouldAdd;
            set
            {
                _shouldAdd = value;
                OnPropertyChanged(nameof(ShouldAdd));
            }
        }

        public bool ShouldDelete
        {
            get => _shouldDelete;
            set
            {
                _shouldDelete = value;
                OnPropertyChanged(nameof(ShouldDelete));
            }
        }

        public bool ShouldUpdate
        {
            get => _shouldUpdate;
            set
            {
                _shouldUpdate = value;
                OnPropertyChanged(nameof(ShouldUpdate));
            }
        }
    }
}