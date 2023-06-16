using System;
using Chami.Db.Entities;
using ChamiUI.PresentationLayer.Minimizing;

namespace ChamiUI.BusinessLayer.Converters
{
    public class MinimizationStrategyConverter : ISettingConverter<IMinimizationStrategy>
    {
        public IMinimizationStrategy Convert(Setting setting)
        {
            if (setting.Value ==  typeof(MinimizeToTaskbarStrategy).FullName)
            {
                return MinimizeToTaskbarStrategy.Instance;
            }

            if (setting.Value == typeof(MinimizeToTrayStrategy).FullName)
            {
                return MinimizeToTrayStrategy.Instance;
            }

            throw new InvalidOperationException($"Invalid type to convert to. ({setting.Type})");
        }
    }
}