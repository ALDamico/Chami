using System.Threading.Tasks;
using ChamiUI.BusinessLayer.Adapters;

namespace ChamiUI.BusinessLayer.MassUpdateStrategies
{
    public interface IMassUpdateStrategy
    {
        void ExecuteUpdate(EnvironmentDataAdapter dataAdapter);
        Task ExecuteUpdateAsync(EnvironmentDataAdapter dataAdapter);
        string VariableName { get; set; }
        string VariableValue { get; set; }
        bool CreateMissing { get; set; }
    }
}