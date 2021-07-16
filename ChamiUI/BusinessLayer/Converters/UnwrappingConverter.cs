using System;
using System.Reflection;
using Chami.Db.Entities;

namespace ChamiUI.BusinessLayer.Converters
{
    /// <summary>
    /// Abstract base class for converters that can perform their operations by constructing an object based on a string containing its fully-qualified type name.
    /// </summary>
    /// <typeparam name="T">Target type that must have a publicly-accessible, parameterless constructor.</typeparam>
    public abstract class UnwrappingConverter<T>
    {
        /// <summary>
        /// Constructs a new object of type <see cref="T"/> and returns it.
        /// </summary>
        /// <param name="setting">The setting to convert from.</param>
        /// <returns>An object of type T, constructed using its public default constructor.</returns>
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