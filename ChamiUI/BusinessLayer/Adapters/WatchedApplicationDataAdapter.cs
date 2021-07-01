using ChamiUI.BusinessLayer.Converters;
using ChamiUI.PresentationLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chami.Db.Entities;
using Chami.Db.Repositories;

namespace ChamiUI.BusinessLayer.Adapters
{
    /// <summary>
    /// Data adapter for the <see cref="WatchedApplicationViewModel"/> class.
    /// </summary>
    public class WatchedApplicationDataAdapter
    {
        /// <summary>
        /// Constructs a new <see cref="WatchedApplicationDataAdapter"/> object with the provided connection string.
        /// </summary>
        /// <param name="connectionString"></param>
        public WatchedApplicationDataAdapter(string connectionString)
        {
            _repository = new WatchedApplicationRepository(connectionString);
        }

        private WatchedApplicationRepository _repository;

        /// <summary>
        /// Gets all the <see cref="WatchedApplication"/> objects whose state is active and converts them to <see cref="WatchedApplicationViewModel"/>s.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of all <see cref="WatchedApplicationViewModel"/>s.</returns>
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

        /// <summary>
        /// Converts a <see cref="WatchedApplicationViewModel"/> to <see cref="WatchedApplication"/> and inserts it in the datastore.
        /// </summary>
        /// <param name="watchedApplication">The <see cref="WatchedApplicationViewModel"/> to convert and insert.</param>
        public void InsertWatchedApplication(WatchedApplicationViewModel watchedApplication)
        {
            var converter = new WatchedApplicationConverter();
            var entity = converter.From(watchedApplication);
            _repository.InsertWatchedApplication(entity);
        }

        /// <summary>
        /// Converts a <see cref="WatchedApplicationViewModel"/> object to a <see cref="WatchedApplication"/> entity and updates the corresponding object in the datastore.
        /// </summary>
        /// <param name="watchedApplication">The <see cref="WatchedApplicationViewModel"/> to update.</param>
        public void UpdateWatchedApplication(WatchedApplicationViewModel watchedApplication)
        {
            var converter = new WatchedApplicationConverter();
            var entity = converter.From(watchedApplication);
            _repository.UpdateApplication(entity);
        }

        /// <summary>
        /// Converts a <see cref="WatchedApplicationViewModel"/> to a <see cref="WatchedApplication"/> entity and inserts it if it's not yet been persisted. Otherwise, updates the existing entity.
        /// </summary>
        /// <param name="watchedApplications"></param>
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
