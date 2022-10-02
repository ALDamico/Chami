using System;
using System.Threading;
using System.Threading.Tasks;
using ChamiUI.Localization;
using ChamiUI.Windows.MainWindow;
using Serilog;

namespace ChamiUI.PresentationLayer.ViewModels.State;

public class MainWindowRevertingEnvironmentState : IMainWindowState
{
    public MainWindowRevertingEnvironmentState(string environmentName)
    {
        EnvironmentName = environmentName;
    }
    public bool EditingEnabled => false;
    public bool IsChangeInProgress => true;
    public bool ExecuteButtonPlayEnabled => false;
    public string ExecuteButtonIcon => "/Assets/Svg/play_disabled.svg";
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
        await Task.CompletedTask;
    }
}