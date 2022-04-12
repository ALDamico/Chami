using System;
using System.Collections.Generic;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;
using dotenv.net;

namespace ChamiUI.BusinessLayer
{
    /// <summary>
    /// Reads an environment from a dotEnv file.
    /// </summary>
    public class EnvironmentDotEnvReader : IEnvironmentReader<EnvironmentViewModel>
    {
        private readonly string _inputFile;

        /// <summary>
        /// Constructs a new <see cref="EnvironmentDotEnvReader"/> object.
        /// </summary>
        /// <param name="inputFile"></param>
        public EnvironmentDotEnvReader(string inputFile)
        {
            _inputFile = inputFile;
        }

        /// <summary>
        /// Reads the file and returns a new <see cref="EnvironmentViewModel"/>.
        /// </summary>
        /// <returns>An <see cref="EnvironmentViewModel"/> with variables read from the dotEnv file.</returns>
        public EnvironmentViewModel Process()
        {
            var newVariables = DotEnv.Fluent().WithEnvFiles(_inputFile).Read();
            var environmentViewModel = new EnvironmentViewModel();
            environmentViewModel.Name = _inputFile;
            foreach (var variable in newVariables)
            {
                var environmentVariable = new EnvironmentVariableViewModel();
                environmentVariable.Name = variable.Key;
                environmentVariable.Value = variable.Value;

                if (environmentVariable.Name == "_CHAMI_ENV")
                {
                    environmentViewModel.Name = environmentVariable.Value;
                }
                else
                {
                    environmentViewModel.EnvironmentVariables.Add(environmentVariable);
                }
            }

            return environmentViewModel;
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <returns>N/A</returns>
        /// <exception cref="NotSupportedException">This action is not supported when reading dotEnv files.</exception>
        public ICollection<EnvironmentViewModel> ProcessMultiple()
        {
            throw new NotSupportedException(ChamiUIStrings.FeatureNotSupportedExceptionMessage);
        }
    }
}