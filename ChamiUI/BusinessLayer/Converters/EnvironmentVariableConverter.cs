using ChamiUI.DataLayer.Entities;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Converters
{
    public class EnvironmentVariableConverter : IConverter<EnvironmentVariable, EnvironmentVariableViewModel>
    {
        public EnvironmentVariable FromModel(EnvironmentVariableViewModel model)
        {
            var environmentVariable = new EnvironmentVariable();
            environmentVariable.Name = model.Name;
            environmentVariable.Value = model.Value;
            environmentVariable.EnvironmentVariableId = model.Id;
            return environmentVariable;
        }

        public EnvironmentVariableViewModel FromEntity(EnvironmentVariable entity)
        {
            var viewModel = new EnvironmentVariableViewModel();
            viewModel.Name = entity.Name;
            viewModel.Value = entity.Value;
            viewModel.Id = entity.EnvironmentVariableId;
            return viewModel;
        }
    }
}