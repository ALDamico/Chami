using Chami.Db.Entities;
using ChamiUI.BusinessLayer.Validators;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Converters
{
    /// <summary>
    /// Converts to and from <see cref="Environment"/>s and <see cref="EnvironmentViewModel"/>s.
    /// </summary>
    public class EnvironmentConverter : IConverter<Environment, EnvironmentViewModel>
    {
        /// <summary>
        /// Converts an <see cref="EnvironmentViewModel"/> to an <see cref="Environment"/> entity.
        /// </summary>
        /// <param name="model">The <see cref="EnvironmentViewModel"/> to convert.</param>
        /// <returns>A converted <see cref="Environment"/> entity.</returns>
        public Environment From(EnvironmentViewModel model)
        {
            var environment = new Environment();
            environment.Name = model.Name;
            environment.AddedOn = model.AddedOn;
            environment.EnvironmentId = model.Id;
            environment.EnvironmentType = model.EnvironmentType;

            var variableConverter = new EnvironmentVariableConverter();
            foreach (var variable in model.EnvironmentVariables)
            {
                var variableEntityValidator = new EnvironmentVariableValidator();
                var isValid = variableEntityValidator.Validate(variable);
                if (isValid.IsValid)
                {
                    var convertedVariable = variableConverter.From(variable);
                    environment.EnvironmentVariables.Add(convertedVariable);
                }
            }

            return environment;
        }

        /// <summary>
        /// Converts an <see cref="Environment"/> to an <see cref="EnvironmentViewModel"/>.
        /// </summary>
        /// <param name="entity">The <see cref="Environment"/> entity to convert.</param>
        /// <returns>A converted <see cref="EnvironmentViewModel"/></returns>
        public EnvironmentViewModel To(Environment entity)
        {
            var viewModel = new EnvironmentViewModel();
            viewModel.Id = entity.EnvironmentId;
            viewModel.Name = entity.Name;
            viewModel.AddedOn = entity.AddedOn;
            viewModel.EnvironmentType = entity.EnvironmentType;

            var variableConverter = new EnvironmentVariableConverter();
            foreach (var variable in entity.EnvironmentVariables)
            {
                var convertedEntity = variableConverter.To(variable);
                convertedEntity.Environment = viewModel;
                viewModel.EnvironmentVariables.Add(convertedEntity);
            }

            return viewModel;
        }
    }
}