using System;
using System.Reflection;
using System.Windows.Documents.Serialization;
using Chami.Db.Entities;
using ChamiUI.PresentationLayer.Minimizing;

namespace ChamiUI.BusinessLayer.Converters
{
    public class MinimizationStrategyConverter
    {
        public IMinimizationStrategy Convert(Setting setting)
        {
            if (setting.SettingName == "MinimizationStrategy")
            {
                var wrappedObject = Activator.CreateInstance(Assembly.GetExecutingAssembly().FullName, setting.Value);
                if (wrappedObject != null)
                {
                    return wrappedObject.Unwrap() as IMinimizationStrategy;
                }
            }

            return null;
        }
    }
}