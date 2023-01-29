using System;
using ChamiUI.BusinessLayer.Converters;
using ChamiUI.BusinessLayer.Validators;
using ChamiUI.PresentationLayer.ViewModels;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Chami.Db.Entities;
using Chami.Db.Repositories;
using ChamiUI.Localization;
using Environment = Chami.Db.Entities.Environment;

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
            _environmentConverter = new EnvironmentConverter();
            _environmentBlackListConverter = new EnvironmentVariableBlacklistConverter();
            _categoryConverter = new CategoryConverter();
        }

        private readonly EnvironmentRepository _repository;
        private readonly IConverter<Environment, EnvironmentViewModel> _environmentConverter;
        private readonly IConverter<EnvironmentVariableBlacklist, EnvironmentVariableBlacklistViewModel>
            _environmentBlackListConverter;
        private readonly CachedConverter<Category, CategoryViewModel> _categoryConverter;

        /// <summary>
        /// Saves a new template <see cref="EnvironmentViewModel"/> to the datastore.
        /// </summary>
        public EnvironmentViewModel SaveTemplateEnvironment(EnvironmentViewModel environment)
        {
            var entity = _environmentConverter.From(environment);
            entity.EnvironmentType = EnvironmentType.TemplateEnvironment;

            _repository.InsertEnvironment(entity);

            return _environmentConverter.To(entity);
        }


        /// <summary>
        /// Get the <see cref="EnvironmentViewModel"/> with the specified id.
        /// </summary>
        /// <param name="id">The id of the <see cref="EnvironmentViewModel"/> to retrieve.</param>
        /// <returns>If an <see cref="Chami.Db.Entities.Environment"/> with the specified id is found, converts it to an <see cref="EnvironmentViewModel"/>. If not, returns null.</returns>
        public EnvironmentViewModel GetEnvironmentById(int id)
        {
            var result = _repository.GetEnvironmentById(id);
            if (result == null)
            {
                return null;
            }
            
            return _environmentConverter.To(result);
        }

        /// <summary>
        /// Gets all the <see cref="EnvironmentViewModel"/>s marked as normal environments in the datastore.
        /// </summary>
        /// <returns>A <see cref="ICollection{T}"/> of <see cref="EnvironmentViewModel"/>s.</returns>
        public ICollection<EnvironmentViewModel> GetEnvironments()
        {
            var models = _repository.GetEnvironments();

            return models.Select(model => _environmentConverter.To(model)).ToList();
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

            return _environmentConverter.To(environment);
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
                var converted = _environmentConverter.From(environmentViewModel);
                var inserted = _repository.InsertEnvironment(converted);
                return _environmentConverter.To(inserted);
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
            var entity = _environmentConverter.From(environment);
            var inserted = _repository.UpsertEnvironment(entity);
            return _environmentConverter.To(inserted);
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
            foreach (var environment in environments)
            {
                output.Add(_environmentConverter.To(environment));
            }

            return output;
        }

        public IEnumerable<EnvironmentViewModel> GetBackupEnvironments()
        {
            var environments = _repository.GetBackupEnvironments();
            var output = new List<EnvironmentViewModel>();
            foreach (var environment in environments)
            {
                output.Add(_environmentConverter.To(environment));
            }

            return output;
        }

        public async Task<ICollection<EnvironmentVariableBlacklistViewModel>> GetBlacklistedVariablesAsync()
        {
            var blacklistedVariables = await _repository.GetBlacklistedVariablesAsync();
            var outputList = new List<EnvironmentVariableBlacklistViewModel>();

            foreach (var blacklistedVariable in blacklistedVariables)
            {
                var viewModel = _environmentBlackListConverter.To(blacklistedVariable);
                outputList.Add(viewModel);
            }

            return outputList;
        }

        public async Task<EnvironmentVariableBlacklistViewModel> SaveBlacklistedVariableAsync(
            EnvironmentVariableBlacklistViewModel blacklistedVariable)
        {
            var entity = _environmentBlackListConverter.From(blacklistedVariable);

            await _repository.UpsertBlacklistedVariableAsync(entity);

            return _environmentBlackListConverter.To(entity);
        }

        public async Task<IEnumerable<string>> GetVariableNamesAsync()
        {
            return await _repository.GetVariableNamesAsync();
        }

        public async Task UpdateVariableByNameAsync(string variableName, string variableValue)
        {
            await _repository.UpdateVariableByNameAsync(variableName, variableValue);
        }

        public async Task UpdateVariableByNameAndEnvironmentIdsAsync(string variableName, string variableValue,
            IEnumerable<EnvironmentViewModel> environments)
        {
            var environmentViewModels = environments.ToList();
            if (environments == null || !environmentViewModels.Any())
            {
                throw new InvalidOperationException(ChamiUIStrings.NoEnvironmentsToUpdateErrorMessage);
            }
            var environmentIds = environmentViewModels.Select(e => e.Id).ToImmutableList();
            
            await _repository.UpdateVariableByNameAndEnvironmentIdsAsync(variableName, variableValue, environmentIds);
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategoryViewModelsAsync()
        {
            var categories = await _repository.GetAllCategoriesAsync();

            return categories.Select(c => _categoryConverter.To(c));
        }

        public IEnumerable<CategoryViewModel> GetAllCategoryViewModels()
        {
            return GetAllCategoryViewModelsAsync().GetAwaiter().GetResult();
        }

        public async Task<CategoryViewModel> UpsertCategoryAsync(CategoryViewModel categoryViewModel)
        {
            var entity = _categoryConverter.From(categoryViewModel);
            var inserted = await _repository.InsertCategoryAsync(entity);
            return _categoryConverter.To(inserted);
        }

        public CategoryViewModel UpsertCategory(CategoryViewModel categoryViewModel)
        {
            return UpsertCategoryAsync(categoryViewModel).GetAwaiter().GetResult();
        }

        public async Task<bool> DeleteCategoryAsync(CategoryViewModel categoryViewModel)
        {
            return await _repository.DeleteCategoryByIdAsync(categoryViewModel.Id.GetValueOrDefault());
        }

        public bool DeleteCategory(CategoryViewModel categoryViewModel)
        {
            return DeleteCategoryAsync(categoryViewModel).GetAwaiter().GetResult();
        }
    }
}