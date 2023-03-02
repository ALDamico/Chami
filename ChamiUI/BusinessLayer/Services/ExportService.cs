using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.BusinessLayer.Converters;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Services;

public class ExportService
{
    public ExportService(EnvironmentDataAdapter environmentDataAdapter,
        EnvironmentExportConverter environmentExportConverter)
    {
        _environmentDataAdapter = environmentDataAdapter;
        _environmentExportConverter = environmentExportConverter;
    }

    public async Task<List<EnvironmentViewModel>> GetEnvironmentsFromDataAdapter(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            cancellationToken.ThrowIfCancellationRequested();
        }

        var environments = await _environmentDataAdapter.GetEnvironmentsAsync(cancellationToken);
        return environments.ToList();
    }

    public async Task<List<EnvironmentViewModel>> GetEnvironmentsByIdsFromDataAdapter(IEnumerable<int> idList)
    {
        var output = new List<EnvironmentViewModel>();

        foreach (var environmentId in idList)
        {
            output.Add(await _environmentDataAdapter.GetEnvironmentByIdAsync(environmentId));
        }

        return output;
    }

    public async Task<ObservableCollection<EnvironmentExportWindowViewModel>> GetExportableEnvironments(
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var data = await GetEnvironmentsFromDataAdapter(cancellationToken);
        var observableCollection = new ObservableCollection<EnvironmentExportWindowViewModel>();
        foreach (var element in data)
        {
            observableCollection.Add(_environmentExportConverter.From(element));
        }

        return observableCollection;
    }

    private readonly EnvironmentDataAdapter _environmentDataAdapter;
    private readonly EnvironmentExportConverter _environmentExportConverter;
}