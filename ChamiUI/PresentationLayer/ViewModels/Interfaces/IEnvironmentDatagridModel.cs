namespace ChamiUI.PresentationLayer.ViewModels.Interfaces
{
    public interface IEnvironmentDatagridModel
    {
        EnvironmentVariableViewModel SelectedVariable { get; set; }
        void OpenFolder();
        void DeleteSelectedVariable();
    }
}