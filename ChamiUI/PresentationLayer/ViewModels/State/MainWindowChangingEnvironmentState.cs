using System.Threading.Tasks;
using ChamiUI.Localization;
using ChamiUI.Windows.MainWindow;

namespace ChamiUI.PresentationLayer.ViewModels.State;

public class MainWindowChangingEnvironmentState : IMainWindowState
{
    public MainWindowChangingEnvironmentState(string environmentName)
    {
        EnvironmentName = environmentName;
    }
    public bool EditingEnabled => false;
    public bool IsChangeInProgress => true;
    public bool ExecuteButtonPlayEnabled => true;
    public string ExecuteButtonIcon => "/Assets/Svg/stop.svg";
    public bool IsDatagridReadonly => true;

    public string WindowStatusMessage =>
        string.Format(ChamiUIStrings.WindowStatusMessageChangeInProgress, EnvironmentName);
    public bool CanDeleteEnvironment => false;
    public bool CanDuplicateEnvironment => false;
    public bool CanExecuteMassUpdate => false;
    
    public bool CanExecuteHealthCheck => false;
    public bool CanSave => false;
    public bool CanImportData => false;
    public bool IsEditable => false;
    public string EnvironmentName { get; }
    public async Task ApplyButtonBehaviour(MainWindowViewModel mainWindowViewModel, MainWindow mainWindow)
    {
        mainWindowViewModel.CancelActiveTask();
        await Task.CompletedTask;
    }

    public bool CanRunApplication => false;
}