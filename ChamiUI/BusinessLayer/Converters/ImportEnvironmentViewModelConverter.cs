using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Converters
{
    public class ImportEnvironmentViewModelConverter : IConverter<EnvironmentViewModel, ImportEnvironmentViewModel>
    {
        public EnvironmentViewModel From(ImportEnvironmentViewModel model)
        {
            return model;
        }

        public ImportEnvironmentViewModel To(EnvironmentViewModel entity)
        {
            var output = new ImportEnvironmentViewModel()
            {
                AddedOn = entity.AddedOn,
                EnvironmentType = entity.EnvironmentType,
                Id = entity.Id,
                IsActive = entity.IsActive,
                Name = entity.Name,
                ShouldImport = true
            };

            foreach (var variable in entity.EnvironmentVariables)
            {
                output.EnvironmentVariables.Add(variable);
            }

            return output;
        }
    }
}