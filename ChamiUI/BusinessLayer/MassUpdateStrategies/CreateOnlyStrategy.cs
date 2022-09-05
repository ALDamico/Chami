using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.MassUpdateStrategies;

public class CreateOnlyStrategy : IMassUpdateStrategy
{
    public CreateOnlyStrategy(string variableName, IEnumerable<EnvironmentViewModel> environments)
    {
        VariableName = variableName;
        TargetEnvironments = environments;
    }

    public void ExecuteUpdate(EnvironmentDataAdapter dataAdapter)
    {
        ExecuteUpdateAsync(dataAdapter).GetAwaiter().GetResult();
    }

    public Task ExecuteUpdateAsync(EnvironmentDataAdapter dataAdapter)
    {
        foreach (var environment in TargetEnvironments)
        {
            if (environment.EnvironmentVariables.Any(v => v.Name == VariableName))
            {
                continue;
            }

            var environmentVariable = new EnvironmentVariableViewModel()
            {
                Environment = environment,
                Name = VariableName
            };
            environment.EnvironmentVariables.Add(environmentVariable);
            dataAdapter.SaveEnvironment(environment);
        }
        
        return Task.CompletedTask;
    }

    public string VariableName { get; set; }
    public string VariableValue { get; set; }
    public bool CreateMissing { get; set; }
    public IEnumerable<EnvironmentViewModel> TargetEnvironments { get; set; }
}