using System;
using System.Reflection;
using Chami.CmdExecutor;
using Chami.Db.Entities;

namespace ChamiUI.BusinessLayer.Factories
{
    /// <summary>
    /// Factory for <see cref="IShellCommand"/> objects.
    /// </summary>
    public static class EnvironmentVariableCommandFactory
    {
        /// <summary>
        /// Creates an appropriate <see cref="IShellCommand"/> based on the requested type.
        /// The <see cref="IShellCommand"/> must implement a public constructor that accepts a parameter of type <see cref="EnvironmentVariable"/>. 
        /// </summary>
        /// <param name="targetType">The class implementing <see cref="IShellCommand"/> to instantiate.</param>
        /// <param name="environmentVariable">The <see cref="EnvironmentVariable"/> object to apply the command to.</param>
        /// <returns>An <see cref="IShellCommand"/> that the <see cref="CmdExecutor"/> object can consume.</returns>
        /// <exception cref="MissingMethodException">The <see cref="IShellCommand"/></exception>
        /// <seealso cref="IShellCommand"/>
        /// <seealso cref="EnvironmentVariableApplicationCommand"/>
        /// <seealso cref="EnvironmentVariableRemovalCommand"/>
        public static Chami.CmdExecutor.IShellCommand GetCommand(Type targetType, EnvironmentVariable environmentVariable)
        {
            ConstructorInfo constructorInfo = targetType.GetConstructor(new[] { typeof(EnvironmentVariable) });
            if (constructorInfo == null)
            {
                throw new MissingMethodException("Could not find a suitable constructor!");
            }


            var obj = constructorInfo.Invoke(new object[] { environmentVariable });
            return obj as IShellCommand;
        }
    }
}