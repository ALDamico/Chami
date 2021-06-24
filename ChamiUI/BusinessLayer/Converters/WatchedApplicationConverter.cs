using Chami.Db.Entities;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Converters
{
    class WatchedApplicationConverter : IConverter<WatchedApplication, WatchedApplicationViewModel>
    {
        public WatchedApplicationViewModel To(WatchedApplication entity)
        {
            var viewModel = new WatchedApplicationViewModel();
            viewModel.Id = entity.Id;
            viewModel.Name = entity.Name;
            viewModel.IsWatchEnabled = entity.IsWatchEnabled;
            return viewModel;
        }

        public WatchedApplication From(WatchedApplicationViewModel model)
        {
            var entity = new WatchedApplication();

            entity.IsWatchEnabled = model.IsWatchEnabled;
            entity.Id = model.Id;
            entity.Name = model.Name;

            return entity;
        }
    }
}
