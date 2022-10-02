namespace ChamiUI.PresentationLayer.ViewModels.State;

public class MainWindowStateManager : ViewModelBase
{
    public IMainWindowState CurrentState
    {
        get => _currentState;
        private set
        {
            _currentState = value;
            OnPropertyChanged();
        }
    }
    private IMainWindowState _currentState;

    public void ChangeState(IMainWindowState newState)
    {
        CurrentState = newState;
    }
}