using System.Collections.ObjectModel;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Converters
{
    public class EnvironmentExportConverter:IConverter<EnvironmentExportWindowViewModel, EnvironmentViewModel>
    {
        public EnvironmentExportWindowViewModel From(EnvironmentViewModel model)
        {
            var newModel = new EnvironmentViewModel()
            {
                Name = model.Name,
                AddedOn = model.AddedOn,
                Id = model.Id
            };
            foreach (var environmentVariable in model.EnvironmentVariables)
            {
                var newVariable = new EnvironmentVariableViewModel()
                {
                    Id = environmentVariable.Id,
                    Name = environmentVariable.Name,
                    Value = environmentVariable.Value,
                    AddedOn = environmentVariable.AddedOn,
                    Environment = newModel
                };
                newModel.EnvironmentVariables.Add(newVariable);
            }
            return new EnvironmentExportWindowViewModel()
            {
                Environment = newModel
            };
        }

        public EnvironmentViewModel To(EnvironmentExportWindowViewModel entity)
        {
            if (entity.Environment == null)
            {
                return null;
            }
            var newModel = new EnvironmentViewModel()
            {
                Name = entity.Environment.Name,
                AddedOn = entity.Environment.AddedOn,
                Id = entity.Environment.Id
            };
            foreach (var environmentVariable in entity.Environment.EnvironmentVariables)
            {
                var newVariable = new EnvironmentVariableViewModel()
                {
                    Id = environmentVariable.Id,
                    Name = environmentVariable.Name,
                    Value = environmentVariable.Value,
                    AddedOn = environmentVariable.AddedOn,
                    Environment = newModel
                };
                newModel.EnvironmentVariables.Add(newVariable);
            }

            return newModel;
        }
    }
}