using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.MassUpdateStrategies
{
    public class UpdateSelectedStrategy : IMassUpdateStrategy
    {
        public UpdateSelectedStrategy(string variableName, string variableValue,
            IEnumerable<EnvironmentViewModel> environmentVariables, bool createMissing)
        {
            VariableName = variableName;
            VariableValue = variableValue;
            Environments = environmentVariables;
            CreateMissing = createMissing;
        }

        public void ExecuteUpdate(EnvironmentDataAdapter dataAdapter)
        {
            ExecuteUpdateAsync(dataAdapter).GetAwaiter().GetResult();
        }

        public async Task ExecuteUpdateAsync(EnvironmentDataAdapter dataAdapter)
        {
            await dataAdapter.UpdateVariableByNameAndEnvironmentIdsAsync(VariableName, VariableValue,
                Environments);

            foreach (var environment in Environments)
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

        public IEnumerable<EnvironmentViewModel> Environments { get; set; }
    }
}