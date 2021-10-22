using ChamiUI.BusinessLayer.Converters;
using ChamiUI.BusinessLayer.Validators;
using ChamiUI.PresentationLayer.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chami.Db.Entities;
using Chami.Db.Repositories;

namespace ChamiUI.BusinessLayer.Adapters
{
    /// <summary>
    /// A data adapter to bridge between the repository layer and the presentation layer with regard to the Environment aggregate.
    /// </summary>
    public class EnvironmentDataAdapter
    {
        /// <summary>
        /// Constructs a new <see cref="EnvironmentDataAdapter"/> object.
        /// </summary>
        /// <param name="connectionString">The connection string to connect to the datastore.</param>
        public EnvironmentDataAdapter(string connectionString)
        {
            _repository = new EnvironmentRepository(connectionString);
        }

        private EnvironmentRepository _repository;

        /// <summary>
        /// Saves a new template <see cref="EnvironmentViewModel"/> to the datastore.
        /// </summary>
        public EnvironmentViewModel SaveTemplateEnvironment(EnvironmentViewModel environment)
        {
            var converter = new EnvironmentConverter();
            var entity = converter.From(environment);
            entity.EnvironmentType = EnvironmentType.TemplateEnvironment;

            _repository.InsertEnvironment(entity);

            return converter.To(entity);
        }


        /// <summary>
        /// Get the <see cref="EnvironmentViewModel"/> with the specified id.
        /// </summary>
        /// <param name="id">The id of the <see cref="EnvironmentViewModel"/> to retrieve.</param>
        /// <returns>If an <see cref="Environment"/> with the specified id is found, converts it to an <see cref="EnvironmentViewModel"/>. If not, returns null.</returns>
        public EnvironmentViewModel GetEnvironmentById(int id)
        {
            var result = _repository.GetEnvironmentById(id);
            if (result == null)
            {
                return null;
            }

            var converter = new EnvironmentConverter();
            return converter.To(result);
        }

        /// <summary>
        /// Gets all the <see cref="EnvironmentViewModel"/>s marked as normal environments in the datastore.
        /// </summary>
        /// <returns>A <see cref="ICollection{T}"/> of <see cref="EnvironmentViewModel"/>s.</returns>
        public ICollection<EnvironmentViewModel> GetEnvironments()
        {
            var models = _repository.GetEnvironments();
            var output = new List<EnvironmentViewModel>();
            var environmentConverter = new EnvironmentConverter();
            foreach (var model in models)
            {
                output.Add(environmentConverter.To(model));
            }

            return output;
        }

        /// <summary>
        /// Gets the <see cref="EnvironmentViewModel"/> with the specified name.
        /// </summary>
        /// <param name="name">The name of the <see cref="EnvironmentViewModel"/> to retrieve.</param>
        /// <returns>If a match is found, returns it. Otherwise, returns null.</returns>
        public EnvironmentViewModel GetEnvironmentByName(string name)
        {
            var environment = _repository.GetEnvironmentByName(name);
            if (environment == null)
            {
                return null;
            }

            return new EnvironmentConverter().To(environment);
        }

        /// <summary>
        /// Gets the <see cref="Environment"/> with the specified id.
        /// </summary>
        /// <param name="id">If a match is found, returns it. Otherwise, returns null.</param>
        /// <returns>The <see cref="Environment"/> with the specified id.</returns>
        public Environment GetEnvironmentEntityById(int id)
        {
            return _repository.GetEnvironmentById(id);
        }

        /// <summary>
        /// Gets the <see cref="Environment"/> with the specified name.
        /// </summary>
        /// <param name="name">The name of the <see cref="Environment"/> to retrieve.</param>
        /// <returns>The <see cref="Environment"/> with the specified name, if found. Otherwise, null.</returns>
        public Environment GetEnvironmentEntityByName(string name)
        {
            return _repository.GetEnvironmentByName(name);
        }

        /// <summary>
        /// Validates the supplied <see cref="EnvironmentViewModel"/>. If the validation is successful, converts to an <see cref="Environment"/> entity and inserts it in the datastore.
        /// </summary>
        /// <param name="environmentViewModel">The <see cref="EnvironmentViewModel"/> to insert.</param>
        /// <returns>If the validation is successful, return a new instance of the <see cref="EnvironmentViewModel"/> with its id updated. Otherwise, returns null.</returns>
        public EnvironmentViewModel InsertEnvironment(EnvironmentViewModel environmentViewModel)
        {
            var validator = new EnvironmentViewModelValidator();
            var validationResult = validator.Validate(environmentViewModel);
            if (validationResult.IsValid)
            {
                var converter = new EnvironmentConverter();
                var converted = converter.From(environmentViewModel);
                var inserted = _repository.InsertEnvironment(converted);
                return converter.To(inserted);
            }

            return null;
        }

        /// <summary>
        /// Deletes the <see cref="Environment"/> corresponding to the supplied <see cref="EnvironmentViewModel"/> from the datastore.
        /// </summary>
        /// <param name="selectedEnvironment">The <see cref="EnvironmentViewModel"/> to delete from the datastore.</param>
        /// <returns>If the <see cref="EnvironmentViewModel"/> is null or its id is 0, returns false. Otherwise, returns whether the deletion was successful or not.</returns>
        public bool DeleteEnvironment(EnvironmentViewModel selectedEnvironment)
        {
            if (selectedEnvironment == null)
            {
                return false;
            }

            int id = selectedEnvironment.Id;
            if (id == 0)
            {
                return false;
            }

            var removed = _repository.DeleteEnvironmentById(id);
            return removed;
        }

        /// <summary>
        /// Converts an <see cref="EnvironmentViewModel"/> to an <see cref="Environment"/> entity and inserts it if it's not been persisted yet. If it has, updates it.
        /// </summary>
        /// <param name="environment">The <see cref="EnvironmentViewModel"/> to save or update.</param>
        /// <returns>The newly-inserted or updated <see cref="Environment"/>, converted to an <see cref="EnvironmentViewModel"/></returns>
        public EnvironmentViewModel SaveEnvironment(EnvironmentViewModel environment)
        {
            var converter = new EnvironmentConverter();
            var entity = converter.From(environment);
            var inserted = _repository.UpsertEnvironment(entity);
            return converter.To(inserted);
        }

        /// <summary>
        /// Performs a backup of the current state of environment variables, excluding those that belong to a Chami environment.
        /// </summary>
        public void BackupEnvironment()
        {
            EnvironmentBackupper.Backup(_repository);
        }

        /// <summary>
        /// Deletes an <see cref="EnvironmentVariableViewModel"/> object from the datastore.
        /// </summary>
        /// <param name="selectedVariable"></param>
        public void DeleteVariable(EnvironmentVariableViewModel selectedVariable)
        {
            _repository.DeleteVariableById(selectedVariable.Id);
        }

        public ICollection<EnvironmentViewModel> GetTemplateEnvironments()
        {
            var environments = _repository.GetTemplateEnvironments();
            
            var output = new List<EnvironmentViewModel>();
            var converter = new EnvironmentConverter();
            foreach (var environment in environments)
            {
                output.Add(converter.To(environment));
            }

            return output;
        }

        public async Task<ICollection<EnvironmentVariableBlacklistViewModel>> GetBlacklistedVariablesAsync()
        {
            var blacklistedVariables = await _repository.GetBlacklistedVariablesAsync();
            var converter = new EnvironmentVariableBlacklistConverter();
            var outputList = new List<EnvironmentVariableBlacklistViewModel>();

            foreach (var blacklistedVariable in blacklistedVariables)
            {
                var viewModel = converter.To(blacklistedVariable);
                outputList.Add(viewModel);
            }

            return outputList;
        }
    }
}