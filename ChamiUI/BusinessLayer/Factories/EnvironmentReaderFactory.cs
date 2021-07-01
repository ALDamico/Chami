using System;
using System.IO;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Factories
{
    /// <summary>
    /// Factory for <see cref="IEnvironmentReader{TOut}"/> objects.
    /// </summary>
    public static class EnvironmentReaderFactory
    {
        /// <summary>
        /// Returns the appropriate <see cref="IEnvironmentReader{TOut}"/> based on the extension of the input file.
        /// </summary>
        /// <param name="filename">The name of the input file.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">No suitable <see cref="IEnvironmentReader{TOut}"/> could be found.</exception>
        /// <seealso cref="IEnvironmentReader{TOut}"/>
        public static IEnvironmentReader<EnvironmentViewModel> GetEnvironmentReaderByExtension(string filename)
        {
            var extension = Path.GetExtension(filename);
            if (extension.Equals(".env", StringComparison.InvariantCultureIgnoreCase))
            {
                return new EnvironmentDotEnvReader(filename);
            }
            else if (extension.Equals(".json", StringComparison.InvariantCultureIgnoreCase))
            {
                return new EnvironmentJsonReader(filename);
            }

            throw new NotSupportedException(ChamiUIStrings.EnvironmentReaderFactoryNotSupportedMessage);
        }
    }
}