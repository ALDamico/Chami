using System;
using Chami.Db.Entities;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Converters
{
    public class EnvironmentVariableBlacklistConverter : IConverter<EnvironmentVariableBlacklist, EnvironmentVariableBlacklistViewModel>
    {
        public EnvironmentVariableBlacklist From(EnvironmentVariableBlacklistViewModel model)
        {
            var output = new EnvironmentVariableBlacklist();
            if (model.Id.HasValue)
            {
                output.Id = model.Id.Value;
            }

            output.Name = model.Name;
            output.InitialValue = model.InitialValue;
            output.IsEnabled = model.IsEnabled;
            output.IsWindowsDefault = model.IsWindowsDefault;
            if (model.AddedOn != null)
            {
                output.AddedOn = model.AddedOn.Value;
            }
            return output;
        }

        public EnvironmentVariableBlacklistViewModel To(EnvironmentVariableBlacklist entity)
        {
            var model = new EnvironmentVariableBlacklistViewModel();

            model.Id = entity.Id;
            model.Name = entity.Name;
            model.InitialValue = entity.InitialValue;
            model.IsWindowsDefault = entity.IsWindowsDefault;
            model.IsEnabled = entity.IsEnabled;
            model.AddedOn = entity.AddedOn;
            
            return model;
        }
    }
}