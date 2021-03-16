using System.Collections.ObjectModel;
using System.Linq;
using ChamiUI.BusinessLayer;
using ChamiUI.BusinessLayer.Factories;
using ChamiUI.DataLayer.Entities;

namespace ChamiUI.PresentationLayer
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(string connectionString)
        {
            _dataAdapter = new EnvironmentDataAdapter(connectionString);
            Environments = GetEnvironments();
            if (Environments.Any())
            {
                SelectedEnvironment = Environments.First();
            }
        }

        public void ChangeEnvironment()
        {
            var cmdExecutor = new CmdExecutor();
            var currentEnvironmentName = System.Environment.GetEnvironmentVariable("_CHAMI_ENV");
            if (currentEnvironmentName != null)
            {
                var currentOsEnvironment = _dataAdapter.GetEnvironmentEntityByName(currentEnvironmentName);
                foreach (var environmentVariable in currentOsEnvironment.EnvironmentVariables)
                {
                    var newCommand =
                        EnvironmentVariableCommandFactory.GetCommand(typeof(EnvironmentVariableRemovalCommand),
                            environmentVariable);
                    cmdExecutor.AddCommand(newCommand);
                }
            }

            var newEnvironment = _dataAdapter.GetEnvironmentEntityByName(SelectedEnvironment.Name);
            cmdExecutor.AddCommand(EnvironmentVariableCommandFactory.GetCommand(
                typeof(EnvironmentVariableApplicationCommand),
                new EnvironmentVariable() {Name = "_CHAMI_ENV", Value = SelectedEnvironment.Name}));

            foreach (var environmentVariable in newEnvironment.EnvironmentVariables)
            {
                var newCommand = EnvironmentVariableCommandFactory.GetCommand(typeof(EnvironmentVariableApplicationCommand),
                    environmentVariable);
                cmdExecutor.AddCommand(newCommand);
            }
            
            cmdExecutor.Execute();
        }

        public ObservableCollection<EnvironmentViewModel> Environments { get; set; }

        private EnvironmentViewModel _selectedEnvironment;

        public EnvironmentViewModel SelectedEnvironment
        {
            get => _selectedEnvironment;

            set
            {
                _selectedEnvironment = value;
                OnPropertyChanged(nameof(SelectedEnvironment));
            }
        }

        public ObservableCollection<EnvironmentViewModel> GetEnvironments()
        {
            return new ObservableCollection<EnvironmentViewModel>(_dataAdapter.GetEnvironments());
        }

        private EnvironmentDataAdapter _dataAdapter;
    }
}