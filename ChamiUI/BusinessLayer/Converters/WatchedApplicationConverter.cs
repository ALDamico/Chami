using ChamiUI.DataLayer.Entities;
using ChamiUI.PresentationLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChamiUI.BusinessLayer.Converters
{
    class WatchedApplicationConverter : IConverter<WatchedApplication, WatchedApplicationViewModel>
    {
        public WatchedApplicationViewModel FromEntity(WatchedApplication entity)
        {
            var viewModel = new WatchedApplicationViewModel();
            viewModel.Id = entity.Id;
            viewModel.Name = entity.Name;
            viewModel.IsWatchEnabled = entity.IsWatchEnabled;
            return viewModel;
        }

        public WatchedApplication FromModel(WatchedApplicationViewModel model)
        {
            var entity = new WatchedApplication();

            entity.IsWatchEnabled = model.IsWatchEnabled;
            entity.Id = model.Id;
            entity.Name = model.Name;

            return entity;
        }
    }
}
