using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Chami.CmdExecutor.Progress;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.Constants;
using ChamiUI.PresentationLayer.Utils;
using ChamiUI.Windows.MainWindow;
using Serilog;

namespace ChamiUI.PresentationLayer.ViewModels.State;

public class MainWindowReadyState : IMainWindowState
{
    public bool EditingEnabled => false;
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
        mainWindowViewModel.ProgressBarViewModel.Reset();
        mainWindowViewModel.SelectedTabIndex = MainWindowConstants.ConsoleTabItem;
        var previousEnvironment = mainWindowViewModel.ActiveEnvironment;
        Action<CmdExecutorProgress> progress = mainWindowViewModel.HandleProgressReport;
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
            mainWindowViewModel.StateManager.ChangeState(new MainWindowRevertingEnvironmentState(previousEnvironment?.Name));
            Log.Logger.Information("{Message}", ex.Message);
            Log.Logger.Information("{StackTrace}", ex.StackTrace);
            mainWindowViewModel.PrintTaskCancelledMessageToConsole();
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

    public async Task CloseMainWindow(MainWindowViewModel mainWindowViewModel, MainWindow mainWindow, CancelEventArgs cancelEventArgs)
    {
        await WindowUtils.CloseWindow(mainWindowViewModel, mainWindow);
    }
}