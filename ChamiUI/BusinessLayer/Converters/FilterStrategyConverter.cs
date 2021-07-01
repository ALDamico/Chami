using System;
using System.Reflection;
using Chami.Db.Entities;
using ChamiUI.PresentationLayer.Filtering;

namespace ChamiUI.BusinessLayer.Converters
{
    public class FilterStrategyConverter : UnwrappingConverter<IFilterStrategy>
    {
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