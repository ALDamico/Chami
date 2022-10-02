using System.Threading.Tasks;
using ChamiUI.Windows.MainWindow;

namespace ChamiUI.PresentationLayer.ViewModels.State;

public interface IMainWindowState
{
    /// <summary>
    /// Determines if the user is in the process of editing an environment.
    /// </summary>
    bool EditingEnabled { get; }
    bool IsChangeInProgress { get; }
    bool ExecuteButtonPlayEnabled { get; }

    /// <summary>
    /// The path to the icon to show in the Execute button.
    /// If no environment is selected the play_disabled image is shown.
    /// If <see cref="ExecuteButtonPlayEnabled"/> is true, the play image is shown.
    /// Otherwise, the stop icon is shown.
    /// </summary>
    string ExecuteButtonIcon { get; }

    bool IsDatagridReadonly { get; }
    string WindowStatusMessage { get; }
    bool CanDeleteEnvironment { get; }
    bool CanDuplicateEnvironment { get; }

    /// <summary>
    /// Determines if the Apply button in the window is enabled (i.e., there's no editing and no environment
    /// switching on progress.
    /// </summary>
    bool CanExecuteMassUpdate { get; }
    
    bool CanExecuteHealthCheck { get; }
    bool CanSave { get; }
    bool CanImportData { get; }
    bool IsEditable { get; }
    Task ApplyButtonBehaviour(MainWindowViewModel mainWindowViewModel, MainWindow mainWindow);
}