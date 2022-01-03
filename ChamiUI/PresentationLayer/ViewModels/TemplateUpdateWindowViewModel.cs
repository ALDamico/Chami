using System.Collections.ObjectModel;
using ChamiUI.Localization;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class TemplateUpdateWindowViewModel : ViewModelBase
    {
        public TemplateUpdateWindowViewModel()
        {
            EnvironmentsToUpdate = new ObservableCollection<EnvironmentUpdateViewModel>();
        }
        private MementoViewModel _updatedEntity;

        public MementoViewModel UpdatedEntity
        {
            get => _updatedEntity;
            set
            {
                _updatedEntity = value;
                OnPropertyChanged(nameof(UpdatedEntity));
            }
        }
        
        public ObservableCollection<EnvironmentUpdateViewModel> EnvironmentsToUpdate { get; }


        public string Description => string.Format(ChamiUIStrings.TemplateUpdateDescription,
            ((UpdatedEntity.EnvironmentViewModel) as EnvironmentViewModel).Name);
    }
}