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

            var variableConverter = new EnvironmentVariableConverter();
            foreach (var variable in entity.EnvironmentVariables)
            {
                viewModel.EnvironmentVariables.Add(variableConverter.To(variable));
            }

            return viewModel;
        }
    }
}