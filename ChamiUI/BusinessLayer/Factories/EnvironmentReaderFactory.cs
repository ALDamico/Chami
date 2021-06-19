using System;
using System.IO;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Factories
{
    public static class EnvironmentReaderFactory
    {
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