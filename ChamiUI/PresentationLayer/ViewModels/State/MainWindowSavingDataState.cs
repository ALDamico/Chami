using System.ComponentModel;
using System.Threading.Tasks;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.Utils;
using ChamiUI.Windows.MainWindow;

namespace ChamiUI.PresentationLayer.ViewModels.State;

public class MainWindowSavingDataState : IMainWindowState
{
    public bool EditingEnabled => false;
    public bool IsChangeInProgress => true;
    public bool ExecuteButtonPlayEnabled => false;
    public string ExecuteButtonIcon => "/Assets/Svg/play_disabled.svg";
    public bool IsDatagridReadonly => true;
    public string WindowStatusMessage => ChamiUIStrings.SavingDataMessage;
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

    public async Task CloseMainWindow(MainWindowViewModel mainWindowViewModel, MainWindow mainWindow, CancelEventArgs cancelEventArgs)
    {
        await WindowUtils.PreventCloseWindow(mainWindowViewModel, mainWindow, cancelEventArgs);
    }
}