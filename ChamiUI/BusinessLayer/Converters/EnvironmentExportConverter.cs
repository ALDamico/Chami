using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Converters
{
    /// <summary>
    /// Converts between <see cref="EnvironmentExportWindowViewModel"/> and <see cref="EnvironmentViewModel"/>
    /// </summary>
    public class EnvironmentExportConverter:IConverter<EnvironmentExportWindowViewModel, EnvironmentViewModel>
    {
        /// <summary>
        /// Converts an <see cref="EnvironmentViewModel"/> to an <see cref="EnvironmentExportWindowViewModel"/> for use in the Chami export window.
        /// </summary>
        /// <param name="model">The <see cref="EnvironmentViewModel"/> to convert from.</param>
        /// <returns>A converted instance of <see cref="EnvironmentExportWindowViewModel"/>.</returns>
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

        /// <summary>
        /// Converts an <see cref="EnvironmentExportWindowViewModel"/> to an <see cref="EnvironmentViewModel"/>.
        /// </summary>
        /// <param name="entity">The <see cref="EnvironmentExportWindowViewModel"/> to convert from.</param>
        /// <returns>A converted instance of <see cref="EnvironmentViewModel"/>.</returns>
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