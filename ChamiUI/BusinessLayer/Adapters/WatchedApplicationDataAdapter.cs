using ChamiUI.BusinessLayer.Converters;
using ChamiUI.PresentationLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChamiDbMigrations.Repositories;

namespace ChamiUI.BusinessLayer.Adapters
{
    public class WatchedApplicationDataAdapter
    {
        public WatchedApplicationDataAdapter(string connectionString)
        {
            _repository = new WatchedApplicationRepository(connectionString);
        }

        private WatchedApplicationRepository _repository;

        public IEnumerable<WatchedApplicationViewModel> GetActiveWatchedApplications()
        {
            var watchedApplications = _repository.GetActiveWatchedApplications();
            var output = new List<WatchedApplicationViewModel>();
            var converter = new WatchedApplicationConverter();
            foreach (var watchedApplication in watchedApplications)
            {
                var viewModel = converter.To(watchedApplication);
                output.Add(viewModel);
            }

            return output;
        }

        public void InsertWatchedApplication(WatchedApplicationViewModel watchedApplication)
        {
            var converter = new WatchedApplicationConverter();
            var entity = converter.From(watchedApplication);
            _repository.InsertWatchedApplication(entity);
        }

        public void UpdateWatchedApplication(WatchedApplicationViewModel watchedApplication)
        {
            var converter = new WatchedApplicationConverter();
            var entity = converter.From(watchedApplication);
            _repository.UpdateApplication(entity);
        }

        public void SaveWatchedApplications(IEnumerable<WatchedApplicationViewModel> watchedApplications)
        {
            var converter = new WatchedApplicationConverter();
            foreach (var watchedApplication in watchedApplications)
            {
                if (watchedApplication.Id == 0)
                {
                    InsertWatchedApplication(watchedApplication);
                }
                else
                {
                    UpdateWatchedApplication(watchedApplication);
                }
            }
        }
    }
}
