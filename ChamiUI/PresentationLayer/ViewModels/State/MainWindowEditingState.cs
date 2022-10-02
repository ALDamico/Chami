using System.Threading.Tasks;
using ChamiUI.Localization;
using ChamiUI.Windows.MainWindow;

namespace ChamiUI.PresentationLayer.ViewModels.State;

public class MainWindowEditingState : IMainWindowState
{
    public bool EditingEnabled => true;
    public bool IsChangeInProgress => false;
    public bool ExecuteButtonPlayEnabled => false;
    public string ExecuteButtonIcon => "/Assets/Svg/play_disabled.svg";
    public bool IsDatagridReadonly => false;
    public string WindowStatusMessage => ChamiUIStrings.WindowStatusMessageEditingMode;
    public bool CanDeleteEnvironment => false;
    public bool CanDuplicateEnvironment => false;
    public bool CanExecuteMassUpdate => false;
    
    public bool CanExecuteHealthCheck => false;
    public bool CanSave => true;
    public bool CanImportData => false;
    public bool IsEditable => false;
    public async Task ApplyButtonBehaviour(MainWindowViewModel mainWindowViewModel, MainWindow mainWindow)
    {
        await Task.CompletedTask;
    }
}