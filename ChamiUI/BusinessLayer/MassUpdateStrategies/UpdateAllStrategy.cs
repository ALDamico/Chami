using System.Linq;
using System.Threading.Tasks;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.MassUpdateStrategies
{
    public class UpdateAllStrategy : IMassUpdateStrategy
    {
        public UpdateAllStrategy(string variableName, string variableValue, bool createMissing)
        {
            VariableName = variableName;
            VariableValue = variableValue;
            CreateMissing = createMissing;
        }
        public void ExecuteUpdate(EnvironmentDataAdapter dataAdapter)
        {
            dataAdapter.UpdateVariableByNameAsync(VariableName, VariableValue).GetAwaiter().GetResult();
        }

        public async Task ExecuteUpdateAsync(EnvironmentDataAdapter dataAdapter)
        {
            await dataAdapter.UpdateVariableByNameAsync(VariableName, VariableValue);
            var environments = dataAdapter.GetEnvironments();
            
            foreach (var environment in environments)
            {
                if (environment.EnvironmentVariables.Any(v => v.Name == VariableName)) continue;
                var newVariable = new EnvironmentVariableViewModel()
                {
                    Environment = environment,
                    Name = VariableName,
                    Value = VariableValue
                };
                environment.EnvironmentVariables.Add(newVariable);
                dataAdapter.SaveEnvironment(environment);
            }
        }

        public string VariableName { get; set; }
        public string VariableValue { get; set; }
        public bool CreateMissing { get; set; }
    }
}