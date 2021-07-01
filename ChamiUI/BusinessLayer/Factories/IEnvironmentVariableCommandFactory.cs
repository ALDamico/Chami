using System;
using System.Reflection;
using Chami.Db.Entities;

namespace ChamiUI.BusinessLayer.Factories
{
    /// <summary>
    /// Factory for <see cref="IEnvironmentVariableCommand"/> objects.
    /// </summary>
    public static class EnvironmentVariableCommandFactory
    {
        /// <summary>
        /// Creates an appropriate <see cref="IEnvironmentVariableCommand"/> based on the requested type.
        /// The <see cref="IEnvironmentVariableCommand"/> must implement a public constructor that accepts a parameter of type <see cref="EnvironmentVariable"/>. 
        /// </summary>
        /// <param name="targetType">The class implementing <see cref="IEnvironmentVariableCommand"/> to instantiate.</param>
        /// <param name="environmentVariable">The <see cref="EnvironmentVariable"/> object to apply the command to.</param>
        /// <returns>An <see cref="IEnvironmentVariableCommand"/> that the <see cref="CmdExecutor"/> object can consume.</returns>
        /// <exception cref="MissingMethodException">The <see cref="IEnvironmentVariableCommand"/></exception>
        /// <seealso cref="IEnvironmentVariableCommand"/>
        /// <seealso cref="EnvironmentVariableApplicationCommand"/>
        /// <seealso cref="EnvironmentVariableRemovalCommand"/>
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