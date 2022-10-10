using Chami.Db.Entities;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Converters
{
    /// <summary>
    /// Converts between <see cref="WatchedApplication"/> entities and <see cref="WatchedApplicationViewModel"/>s.
    /// </summary>
    class WatchedApplicationConverter : IConverter<WatchedApplication, WatchedApplicationViewModel>
    {
        /// <summary>
        /// Converts a <see cref="WatchedApplication"/> entity to a <see cref="WatchedApplicationViewModel"/>.
        /// </summary>
        /// <param name="entity">The entity to convert from.</param>
        /// <returns>A converted <see cref="WatchedApplicationViewModel"/>.</returns>
        public WatchedApplicationViewModel To(WatchedApplication entity)
        {
            var viewModel = new WatchedApplicationViewModel();
            viewModel.Id = entity.Id;
            viewModel.Name = entity.Name;
            viewModel.IsWatchEnabled = entity.IsWatchEnabled;
            viewModel.Icon = entity.ApplicationIcon;
            viewModel.Path = entity.Path;
            viewModel.IconHeight = entity.IconHeight;
            viewModel.IconWidth = entity.IconWidth;
            return viewModel;
        }

        /// <summary>
        /// Converts a <see cref="WatchedApplicationViewModel"/> to a <see cref="WatchedApplication"/> entity.
        /// </summary>
        /// <param name="model">The viewmodel to convert from.</param>
        /// <returns>A converted <see cref="WatchedApplication"/> entity.</returns>
        public WatchedApplication From(WatchedApplicationViewModel model)
        {
            var entity = new WatchedApplication();

            entity.IsWatchEnabled = model.IsWatchEnabled;
            entity.Id = model.Id;
            entity.Name = model.Name;
            entity.Path = model.Path;
            entity.ApplicationIcon = model.Icon;
            entity.IconHeight = model.IconHeight;
            entity.IconWidth = model.IconWidth;
            return entity;
        }
    }
}
