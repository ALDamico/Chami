using System;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.EnvironmentHealth.Strategies
{
    public class DefaultHealthCheckerStrategy : IEnvironmentHealthCheckerStrategy
    {
        public double CheckHealth(EnvironmentViewModel environment, EnvironmentHealthCheckerConfiguration configuration)
        {
            if (environment == null)
            {
                return 0;
            }

            var registryRetriever = new EnvironmentVariableRegistryRetriever();
            var systemEnvironmentVariables = registryRetriever.GetEnvironmentVariables();
            int found = 0;
            int mismatchedValue = 0;

            foreach (var environmentVariable in environment.EnvironmentVariables)
            {
                if (environmentVariable.Name == null)
                {
                    continue;
                }
                var variableName = environmentVariable.Name;
                var variableValue = environmentVariable.Value;
                var exists = systemEnvironmentVariables.TryGetValue(variableName, out var correspondingVariable);

                if (exists)
                {
                    found++;
                    if (correspondingVariable != variableValue)
                    {
                        mismatchedValue++;
                    }
                }
            }

            var environmentVariablesCount = environment.EnvironmentVariables.Count;

            var temporaryFoundResult = 0.0;
            if (environmentVariablesCount > 0)
            {
                temporaryFoundResult = found * configuration.MaxScore / environmentVariablesCount;
            }
            
            for (int i = 0; i < mismatchedValue; i++)
            {
                temporaryFoundResult -= configuration.MismatchPenalty;
            }

            return Math.Max(0.0, temporaryFoundResult);
        }
    }
}