using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Converters
{
    public class EnvironmentUpdateViewModelConverter : IConverter<EnvironmentViewModel, EnvironmentUpdateViewModel>
    {
        public EnvironmentViewModel From(EnvironmentUpdateViewModel model)
        {
            return model;
        }

        public EnvironmentUpdateViewModel To(EnvironmentViewModel entity)
        {
            var output = new EnvironmentUpdateViewModel();

            output.Id = entity.Id;
            output.Name = entity.Name;
            output.AddedOn = entity.AddedOn;
            output.EnvironmentType = entity.EnvironmentType;
            output.IsActive = entity.IsActive;
            foreach (var variable in entity.EnvironmentVariables)
            {
                output.EnvironmentVariables.Add(variable);
            }
            return output;
        }
    }
}