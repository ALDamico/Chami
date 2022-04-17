using System.Threading.Tasks;
using ChamiUI.BusinessLayer.Adapters;

namespace ChamiUI.BusinessLayer.MassUpdateStrategies
{
    public class UpdateAllStrategy : IMassUpdateStrategy
    {
        public UpdateAllStrategy(string variableName, string variableValue)
        {
            VariableName = variableName;
            VariableValue = variableValue;
        }
        public void ExecuteUpdate(EnvironmentDataAdapter dataAdapter)
        {
            dataAdapter.UpdateVariableByNameAsync(VariableName, VariableValue).GetAwaiter().GetResult();
        }

        public async Task ExecuteUpdateAsync(EnvironmentDataAdapter dataAdapter)
        {
            await dataAdapter.UpdateVariableByNameAsync(VariableName, VariableValue);
        }

        public string VariableName { get; set; }
        public string VariableValue { get; set; }
    }
}