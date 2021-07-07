using System;
using Chami.Db.Entities;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Converters
{
    /// <summary>
    /// Converts between <see cref="EnvironmentVariable"/>s and <see cref="EnvironmentVariableViewModel"/>s.
    /// </summary>
    public class EnvironmentVariableConverter : IConverter<EnvironmentVariable, EnvironmentVariableViewModel>
    {
        /// <summary>
        /// Converts an <see cref="EnvironmentVariableViewModel"/> to a <see cref="EnvironmentVariable"/> entity.
        /// </summary>
        /// <param name="model">The <see cref="EnvironmentVariableViewModel"/> to convert from.</param>
        /// <returns>A converted instance of <see cref="EnvironmentVariable"/>.</returns>
        public EnvironmentVariable From(EnvironmentVariableViewModel model)
        {
            var environmentVariable = new EnvironmentVariable();
            environmentVariable.Name = model.Name;
            environmentVariable.Value = model.Value;
            environmentVariable.EnvironmentVariableId = model.Id;
            environmentVariable.MarkedForDeletion = model.MarkedForDeletion;
            return environmentVariable;
        }
        
        /// <summary>
        /// Converts an <see cref="EnvironmentVariable"/> to an <see cref="EnvironmentVariableViewModel"/>.
        /// </summary>
        /// <param name="entity">The <see cref="EnvironmentVariable"/> entity to convert from.</param>
        /// <returns>A converted instance of <see cref="EnvironmentVariableViewModel"/>.</returns>
        public EnvironmentVariableViewModel To(EnvironmentVariable entity)
        {
            var viewModel = new EnvironmentVariableViewModel();
            viewModel.Name = entity.Name;
            viewModel.Value = entity.Value;
            viewModel.Id = entity.EnvironmentVariableId;
            if (entity.MarkedForDeletion)
            {
                viewModel.MarkForDeletion();
            }
            return viewModel;
        }
    }
}