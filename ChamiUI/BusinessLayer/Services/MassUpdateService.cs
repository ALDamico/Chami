using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.BusinessLayer.Factories;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Services;

public class MassUpdateService
{
    public MassUpdateService(EnvironmentDataAdapter environmentDataAdapter)
    {
        _environmentDataAdapter = environmentDataAdapter;
    }

    public IEnumerable<MassUpdateStrategyViewModel> GetAvailableMassUpdateStrategies()
    {
        var updateStrategies = new List<MassUpdateStrategyViewModel>();
        var updateAllStrategy = MassUpdateStrategyViewModel.DefaultUpdateStrategy;
        updateStrategies.Add(updateAllStrategy);

        var updateSelectedStrategy = new MassUpdateStrategyViewModel()
        {
            Name = ChamiUIStrings.MassUpdateStrategyName_UpdateSelected,
            CreateIfNotExistsEnabled = true,
            EnvironmentListBoxEnabled = true
        };
            
        updateStrategies.Add(updateSelectedStrategy);
        var createOnlyStrategy = new MassUpdateStrategyViewModel()
        {
            Name = ChamiUIStrings.MassUpdateStrategyName_CreateOnly,
            CreateIfNotExistsEnabled = false,
            CreateIfNotExists = true,
            EnvironmentListBoxEnabled = false
        };
        updateStrategies.Add(createOnlyStrategy);

        return updateStrategies;
    }

    private async Task<IEnumerable<EnvironmentViewModel>> GetEnvironmentViewModelsAsync(CancellationToken cancellationToken)
    {
        return await _environmentDataAdapter.GetEnvironmentsAsync(cancellationToken);
    }

    private async Task<IEnumerable<string>> GetKnownVariables()
    {
        return await _environmentDataAdapter.GetVariableNamesAsync();
    }

    public List<Task> GetLoadDataTask()
    {
        var tasks = new List<Task>();
        
        tasks.Add(GetEnvironmentViewModelsAsync(new CancellationToken()));
        tasks.Add(GetKnownVariables());

        return tasks;
    }

    public async Task ExecuteUpdate(MassUpdateStrategyViewModel selectedUpdateStrategy, 
        string variableName, 
        string newValue,
        IEnumerable<EnvironmentViewModel> environments, 
        bool createVariableIfNotExists
        )
    {
        var environmentsList = environments.ToList();
        var massUpdateStrategy = MassUpdateStrategyFactory.GetMassUpdateStrategyByViewModel(selectedUpdateStrategy,
            variableName, newValue, environmentsList, createVariableIfNotExists);
        await massUpdateStrategy.ExecuteUpdateAsync(_environmentDataAdapter);
        if (createVariableIfNotExists)
        {
            foreach (var environment in environmentsList)
            {
                environment.EnvironmentVariables.Add(new EnvironmentVariableViewModel()
                    {Environment = environment, Name = variableName, Value = newValue});
                _environmentDataAdapter.SaveEnvironment(environment);
            }
        }
    }

    private readonly EnvironmentDataAdapter _environmentDataAdapter;
}