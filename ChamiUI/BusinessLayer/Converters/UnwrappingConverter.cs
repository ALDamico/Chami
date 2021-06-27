using System;
using System.Reflection;
using Chami.Db.Entities;
using ChamiUI.PresentationLayer.Filtering;

namespace ChamiUI.BusinessLayer.Converters
{
    public abstract class UnwrappingConverter<T>
    {
        public T Convert(Setting setting)
        {
            var settingValue = setting.Value;
            var wrappedInstance =
                Activator.CreateInstance(Assembly.GetExecutingAssembly().FullName, settingValue);
            if (wrappedInstance != null)
            {
                return (T) wrappedInstance.Unwrap();
            }

            return default;
        }
    }
}