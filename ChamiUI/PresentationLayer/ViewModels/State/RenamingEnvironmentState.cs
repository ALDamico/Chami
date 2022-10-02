using System.Threading.Tasks;
using ChamiUI.Localization;
using ChamiUI.Windows.MainWindow;

namespace ChamiUI.PresentationLayer.ViewModels.State;

public class RenamingEnvironmentState : IMainWindowState
{
    public bool EditingEnabled => false;
    public bool IsChangeInProgress => true;
    public bool ExecuteButtonPlayEnabled => false;
    public string ExecuteButtonIcon => "/Assets/Svg/play_disabled.svg";
    public bool IsDatagridReadonly => true;
    public string WindowStatusMessage => ChamiUIStrings.RenamingEnvironmentMessage;
    public bool CanDeleteEnvironment => false;
    public bool CanDuplicateEnvironment => false;
    public bool CanExecuteMassUpdate => false;
    public bool CanExecuteHealthCheck => true;
    public bool CanSave => false;
    public bool CanImportData => false;
    public bool IsEditable => false;
    public async Task ApplyButtonBehaviour(MainWindowViewModel mainWindowViewModel, MainWindow mainWindow)
    {
        await Task.CompletedTask;
    }
}