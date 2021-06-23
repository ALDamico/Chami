using System;
using System.Reflection;
using ChamiDbMigrations.Entities;

namespace ChamiUI.BusinessLayer.Factories
{
    public static class EnvironmentVariableCommandFactory
    {
        public static IEnvironmentVariableCommand GetCommand(Type targetType, EnvironmentVariable environmentVariable)
        {
            ConstructorInfo constructorInfo = targetType.GetConstructor(new[] { typeof(EnvironmentVariable) });
            if (constructorInfo == null)
            {
                throw new MissingMethodException("Could not find a suitable constructor!");
            }


            var obj = constructorInfo.Invoke(new object[] { environmentVariable });
            return obj as IEnvironmentVariableCommand;
        }
    }
}