using System.Threading.Tasks;
using ChamiUI.Localization;
using ChamiUI.Windows.MainWindow;

namespace ChamiUI.PresentationLayer.ViewModels.State;

public class MainWindowNotRunnableState : IMainWindowState
{
    public bool EditingEnabled => false;
    public bool IsChangeInProgress => false;
    public bool ExecuteButtonPlayEnabled => false;
    public string ExecuteButtonIcon => "/Assets/Svg/play_disabled.svg";
    public bool IsDatagridReadonly => true;
    public string WindowStatusMessage => ChamiUIStrings.WindowStatusMessageReady;
    public bool CanDeleteEnvironment => true;
    public bool CanDuplicateEnvironment => true;
    public bool CanExecuteMassUpdate => true;
    
    public bool CanExecuteHealthCheck => true;
    public bool CanSave => false;
    public bool CanImportData => true;
    public bool IsEditable => true;
    public async Task ApplyButtonBehaviour(MainWindowViewModel mainWindowViewModel, MainWindow mainWindow)
    {
        await Task.CompletedTask;
    }

    public bool CanRunApplication => false;
}