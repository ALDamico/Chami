using System.Collections.ObjectModel;
using ChamiUI.Localization;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class MassUpdateWindowViewModel : ViewModelBase
    {
        public MassUpdateWindowViewModel()
        {
            KnownVariables = new ObservableCollection<string>();
            UpdateStrategies = new ObservableCollection<MassUpdateStrategyViewModel>();
            InitUpdateStrategies();
        }

        private void InitUpdateStrategies()
        {
            var updateAllStrategy = new MassUpdateStrategyViewModel()
                { Name = ChamiUIStrings.MassUpdateStrategyName_UpdateAll };
            UpdateStrategies.Add(updateAllStrategy);

            var updateSelectedStrategy = new MassUpdateStrategyViewModel()
                { Name = ChamiUIStrings.MassUpdateStrategyName_UpdateSelected };
            UpdateStrategies.Add(updateSelectedStrategy);
        }
        
        private string _variableToUpdate;
        private bool _createVariableIfNotExists;
        private MassUpdateStrategyViewModel _selectedUpdateStrategy;

        public MassUpdateStrategyViewModel SelectedUpdateStrategy
        {
            get => _selectedUpdateStrategy;
            set
            {
                _selectedUpdateStrategy = value;
                OnPropertyChanged(nameof(SelectedUpdateStrategy));
            }
        }

        public bool CreateVariableIfNotExists
        {
            get => _createVariableIfNotExists;
            set
            {
                _createVariableIfNotExists = value;
                OnPropertyChanged(nameof(CreateVariableIfNotExists));
            }
        }

        public string VariableToUpdate
        {
            get => _variableToUpdate;
            set
            {
                _variableToUpdate = value;
                OnPropertyChanged(nameof(VariableToUpdate));
            }
        }
        
        public ObservableCollection<string> KnownVariables { get; }
        public ObservableCollection<MassUpdateStrategyViewModel> UpdateStrategies { get; }
    }
}