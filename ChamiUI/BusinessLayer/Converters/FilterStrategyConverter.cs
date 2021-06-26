using System;
using System.Reflection;
using Chami.Db.Entities;
using ChamiUI.PresentationLayer.Filtering;

namespace ChamiUI.BusinessLayer.Converters
{
    public class FilterStrategyConverter
    {
        public IFilterStrategy Convert(Setting setting)
        {
            if (setting.SettingName == "SearchPath")
            {
                var filterStrategyName = setting.Value;
                var wrappedInstance =
                    Activator.CreateInstance(Assembly.GetExecutingAssembly().FullName, filterStrategyName);
                if (wrappedInstance != null)
                {
                    return wrappedInstance.Unwrap() as IFilterStrategy;
                }
            }

            return null;
        }

        public string GetSettingValue(IFilterStrategy filterStrategy)
        {
            if (filterStrategy == null)
            {
                return null;
            }

            return filterStrategy.GetType().FullName;
        }
    }
}