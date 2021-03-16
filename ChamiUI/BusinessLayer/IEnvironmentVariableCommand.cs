using ChamiUI.DataLayer.Entities;

namespace ChamiUI.BusinessLayer
{
    public interface IEnvironmentVariableCommand
    {
        EnvironmentVariable EnvironmentVariable { get; set; }
        void Execute();
    }
}