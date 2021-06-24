using Chami.Db.Entities;
using ChamiUI.BusinessLayer.Validators;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Converters
{
    public class EnvironmentConverter : IConverter<Environment, EnvironmentViewModel>
    {
        public Environment From(EnvironmentViewModel model)
        {
            var environment = new Environment();
            environment.Name = model.Name;
            environment.AddedOn = model.AddedOn;
            environment.EnvironmentId = model.Id;

            var variableConverter = new EnvironmentVariableConverter();
            foreach (var variable in model.EnvironmentVariables)
            {
                var variableEntityValidator = new EnvironmentVariableValidator();
                var isValid = variableEntityValidator.Validate(variable);
                if (isValid.IsValid)
                {
                    environment.EnvironmentVariables.Add(variableConverter.From(variable));
                }
            }

            return environment;
        }

        public EnvironmentViewModel To(Environment entity)
        {
            var viewModel = new EnvironmentViewModel();
            viewModel.Id = entity.EnvironmentId;
            viewModel.Name = entity.Name;
            viewModel.AddedOn = entity.AddedOn;

            var variableConverter = new EnvironmentVariableConverter();
            foreach (var variable in entity.EnvironmentVariables)
            {
                viewModel.EnvironmentVariables.Add(variableConverter.To(variable));
            }

            return viewModel;
        }
    }
}