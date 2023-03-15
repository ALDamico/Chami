using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.BusinessLayer.Mementos;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;
using ChamiUI.Utils;
using Serilog;

namespace ChamiUI.BusinessLayer.Services;

public class NewEnvironmentService
{
    public NewEnvironmentService(EnvironmentDataAdapter environmentDataAdapter)
    {
        _environmentDataAdapter = environmentDataAdapter;
        _caretaker = new EnvironmentCaretaker();
        TemplateEnvironments = new ObservableCollection<EnvironmentViewModel>();
        BindingOperations.EnableCollectionSynchronization(TemplateEnvironments, new object());
        RetrieveEnvironmentTemplates().Await();
    }

    private readonly EnvironmentDataAdapter _environmentDataAdapter;

    public async Task SaveEnvironment(EnvironmentViewModel environment)
    {
        try
        {
            await _environmentDataAdapter.SaveEnvironmentAsync(environment);
            EnvironmentSaved?.Invoke(this, new EnvironmentSavedEventArgs(environment));
        }
        catch (SQLiteException ex)
        {
            Log.Logger.Error("{Message}", ex.Message);
            Log.Logger.Error("{StackTrace}", ex.StackTrace);
        }
    }

    public async Task RetrieveEnvironmentTemplates(CancellationToken cancellationToken = default)
    {
        TemplateEnvironments.Clear();
        var templates = await _environmentDataAdapter.GetTemplateEnvironmentsAsync(cancellationToken);
        TemplateEnvironments.Add(new EnvironmentViewModel() {Name = "None"});
        foreach (var template in templates)
        {
            TemplateEnvironments.Add(template);
        }
    }

    public ObservableCollection<EnvironmentViewModel> TemplateEnvironments { get; }

    public event EventHandler<EnvironmentSavedEventArgs> EnvironmentSaved;
    private readonly EnvironmentCaretaker _caretaker;

    public EnvironmentViewModel ChangeTemplate(EnvironmentViewModel previousTemplate, EnvironmentViewModel currentTemplate, EnvironmentViewModel environment)
    {
        EnvironmentViewModel output;
        _caretaker.SaveState(previousTemplate?.Name, environment);
        var state = _caretaker.ResumeState(currentTemplate.Name);
        if (state == null)
        {
            var environmentNew = new EnvironmentViewModel();
            environmentNew.Name = currentTemplate?.Name;
            foreach (var environmentVariable in currentTemplate.EnvironmentVariables)
            {
                environment.EnvironmentVariables.Add(new EnvironmentVariableViewModel()
                {
                    Name = environmentVariable.Name, 
                    Value = environmentVariable.Value, 
                    Environment = environment
                });
            }
            
            currentTemplate.EnvironmentVariables.Clear();
            foreach (var variable in environment.EnvironmentVariables)
            {
                currentTemplate.EnvironmentVariables.Add(variable);
            }
           

            output = environmentNew;
        }
        else
        {
            output = state;
        }

        return output;
    }
}