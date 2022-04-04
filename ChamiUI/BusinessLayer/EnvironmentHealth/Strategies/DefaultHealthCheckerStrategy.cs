using Chami.Db.Entities;
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
                var variableName = environmentVariable.Name;
                var variableValue = environmentVariable.Value;
                var correspondingVariable = systemEnvironmentVariables[variableName];

                if (correspondingVariable != null)
                {
                    found++;
                    if (correspondingVariable != variableValue)
                    {
                        mismatchedValue++;
                    }
                }
            }

            var temporaryFoundResult = found * configuration.MaxScore / environment.EnvironmentVariables.Count;
            for (int i = 0; i < mismatchedValue; i++)
            {
                temporaryFoundResult -= configuration.MismatchPenalty;
            }

            return temporaryFoundResult;
        }
    }
}