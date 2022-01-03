using System.Collections.ObjectModel;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class MementoViewModel : ViewModelBase
    {
        public MementoViewModel()
        {
            AddedItems = new ObservableCollection<EnvironmentVariableViewModel>();
            RemovedItems = new ObservableCollection<EnvironmentVariableViewModel>();
            EditedItems = new ObservableCollection<EnvironmentVariableViewModel>();
        }
        private EnvironmentViewModel _environmentViewModel;

        public EnvironmentViewModel EnvironmentViewModel
        {
            get => _environmentViewModel;
            set
            {
                _environmentViewModel = value;
                OnPropertyChanged(nameof(EnvironmentViewModel));
            }
        }
        
        public ObservableCollection<EnvironmentVariableViewModel> AddedItems { get; }
        public ObservableCollection<EnvironmentVariableViewModel> RemovedItems { get; }
        public ObservableCollection<EnvironmentVariableViewModel> EditedItems { get; }

        public void Clear()
        {
            EnvironmentViewModel = null;
            AddedItems.Clear();
            RemovedItems.Clear();
            EditedItems.Clear();
        }
    }
}