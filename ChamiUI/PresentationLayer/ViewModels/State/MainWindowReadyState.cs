using System;
using System.Threading;
using System.Threading.Tasks;
using Chami.CmdExecutor.Progress;
using ChamiUI.Localization;
using ChamiUI.Windows.MainWindow;
using Serilog;

namespace ChamiUI.PresentationLayer.ViewModels.State;

public class MainWindowReadyState : IMainWindowState
{
    public bool EditingEnabled => true;
    public bool IsChangeInProgress => false;
    public bool ExecuteButtonPlayEnabled => true;
    public string ExecuteButtonIcon => "/Assets/Svg/play.svg";
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
        mainWindow.ResetProgressBar();
        mainWindow.FocusConsoleTab();
        var previousEnvironment = mainWindowViewModel.ActiveEnvironment;
        Action<CmdExecutorProgress> progress = mainWindow.HandleProgressReport;
        try
        {
            await mainWindowViewModel.ChangeEnvironmentAsync(progress);
            var watchedApplicationSettings = mainWindowViewModel.Settings.WatchedApplicationSettings;
            if (watchedApplicationSettings.IsDetectionEnabled)
            {
                mainWindowViewModel.DetectApplicationsAndShowWindow();
            }
        }
        catch (Exception ex) when (ex is TaskCanceledException or OperationCanceledException)
        {
            mainWindowViewModel.StateManager.ChangeState(new MainWindowRevertingEnvironmentState());
            Log.Logger.Information("{Message}", ex.Message);
            Log.Logger.Information("{StackTrace}", ex.StackTrace);
            mainWindow.PrintTaskCancelledMessageToConsole();
            mainWindowViewModel.SelectedEnvironment = previousEnvironment;
            if (previousEnvironment != null)
            {
                await mainWindowViewModel.ChangeEnvironmentAsync(progress);
            }
            else
            {
                await mainWindowViewModel.ResetEnvironmentAsync(progress, CancellationToken.None);
            }
        }
    }
}