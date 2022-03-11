using System.Collections.Generic;
using System.Threading.Tasks;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.MassUpdateStrategies
{
    public class UpdateSelectedStrategy : IMassUpdateStrategy
    {
        public UpdateSelectedStrategy(string variableName, string variableValue, IEnumerable<EnvironmentViewModel> environmentVariables)
        {
            VariableName = variableName;
            VariableValue = variableValue;
            EnvironmentVariableViewModels = environmentVariables;

        }
        public void ExecuteUpdate(EnvironmentDataAdapter dataAdapter)
        {
            ExecuteUpdateAsync(dataAdapter).GetAwaiter().GetResult();
        }

        public async  Task ExecuteUpdateAsync(EnvironmentDataAdapter dataAdapter)
        {
            await dataAdapter.UpdateVariableByNameAndEnvironmentIdsAsync(VariableName, VariableValue,
                EnvironmentVariableViewModels);
        }

        public string VariableName { get; set; }
        public string VariableValue { get; set; }
        
        public IEnumerable<EnvironmentViewModel> EnvironmentVariableViewModels { get; set; }
    }
}