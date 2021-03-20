using System.Collections;
using System.Collections.Generic;
using ChamiUI.BusinessLayer.Converters;
using ChamiUI.BusinessLayer.Validators;
using ChamiUI.DataLayer.Entities;
using ChamiUI.DataLayer.Repositories;
using ChamiUI.PresentationLayer;

namespace ChamiUI.BusinessLayer
{
    public class EnvironmentDataAdapter
    {
        
        public EnvironmentDataAdapter(string connectionString)
        {
            _repository = new EnvironmentRepository(connectionString);
        }
        private EnvironmentRepository _repository;

        public ICollection<EnvironmentViewModel> GetEnvironments()
        {
            var models = _repository.GetEnvironments();
            var output = new List<EnvironmentViewModel>();
            var environmentConverter = new EnvironmentConverter();
            foreach (var model in models)
            {
                output.Add(environmentConverter.FromEntity(model));   
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

            return new EnvironmentConverter().FromEntity(environment);
        }
        
        public Environment GetEnvironmentEntityByName(string name)
        {
            return _repository.GetEnvironmentByName(name);
        }

        public bool InsertEnvironment(EnvironmentViewModel environmentViewModel)
        {
            var validator = new EnvironmentViewModelValidator();
            var validationResult = validator.Validate(environmentViewModel);
            if (validationResult.IsValid)
            {
                var converter = new EnvironmentConverter();
                var converted = converter.FromModel(environmentViewModel);
                var inserted = _repository.InsertEnvironment(converted);
                return inserted != null;
            }

            return false;
        }

        public bool DeleteEnvironment(EnvironmentViewModel selectedEnvironment)
        {
            int id = selectedEnvironment.Id;
            if (id == 0)
            {
                return false;
            }

            var removed = _repository.DeleteEnvironmentById(id);
            return removed;
        }
    }
}