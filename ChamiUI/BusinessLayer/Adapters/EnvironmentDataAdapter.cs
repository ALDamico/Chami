using ChamiUI.BusinessLayer.Converters;
using ChamiUI.BusinessLayer.Validators;
using ChamiUI.PresentationLayer.ViewModels;
using System.Collections.Generic;
using Chami.Db.Entities;
using Chami.Db.Repositories;

namespace ChamiUI.BusinessLayer.Adapters
{
    public class EnvironmentDataAdapter
    {

        public EnvironmentDataAdapter(string connectionString)
        {
            _repository = new EnvironmentRepository(connectionString);
        }
        private EnvironmentRepository _repository;

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

        public EnvironmentViewModel GetEnvironmentByName(string name)
        {
            var environment = _repository.GetEnvironmentByName(name);
            if (environment == null)
            {
                return null;
            }

            return new EnvironmentConverter().To(environment);
        }

        public Environment GetEnvironmentEntityById(int id)
        {
            return _repository.GetEnvironmentById(id);
        }

        public Environment GetEnvironmentEntityByName(string name)
        {
            return _repository.GetEnvironmentByName(name);
        }

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

        public EnvironmentViewModel SaveEnvironment(EnvironmentViewModel environment)
        {
            var converter = new EnvironmentConverter();
            var entity = converter.From(environment);
            var inserted = _repository.UpsertEnvironment(entity);
            return converter.To(inserted);
        }

        public void BackupEnvironment()
        {
            EnvironmentBackupper.Backup(_repository);
        }

        public void DeleteVariable(EnvironmentVariableViewModel selectedVariable)
        {
            _repository.DeleteVariableById(selectedVariable.Id);
        }
    }
}