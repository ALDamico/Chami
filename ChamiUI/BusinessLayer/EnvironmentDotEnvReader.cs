using System;
using System.Collections.Generic;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;
using dotenv.net;

namespace ChamiUI.BusinessLayer
{
    public class EnvironmentDotEnvReader:IEnvironmentReader<EnvironmentViewModel>
    {
        private string _inputFile;
        public EnvironmentDotEnvReader(string inputFile)
        {
            _inputFile = inputFile;
        }
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

        public ICollection<EnvironmentViewModel> ProcessMultiple()
        {
            throw new NotSupportedException(ChamiUIStrings.FeatureNotSupportedExceptionMessage);
        }
    }
}