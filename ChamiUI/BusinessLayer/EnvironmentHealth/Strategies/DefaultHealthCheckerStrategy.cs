using System;
using System.Collections.Generic;
using ChamiUI.BusinessLayer.Validators;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.EnvironmentHealth.Strategies
{
    public class DefaultHealthCheckerStrategy : IEnvironmentHealthCheckerStrategy
    {
        public DefaultHealthCheckerStrategy()
        {
            HealthStatuses = new List<EnvironmentVariableHealthStatus>();
        }
        public double CheckHealth(EnvironmentViewModel environment, EnvironmentHealthCheckerConfiguration configuration)
        {
            HealthStatuses.Clear();
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
                EnvironmentVariableHealthType issueType = EnvironmentVariableHealthType.Ok;
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
                        issueType = EnvironmentVariableHealthType.MismatchedValue;
                    }
                }
                else
                {
                    issueType = EnvironmentVariableHealthType.MissingVariable;
                }

                var variableStatus = new EnvironmentVariableHealthStatus()
                {
                    EnvironmentVariable = environmentVariable,
                    ActualValue = correspondingVariable,
                    ExpectedValue = environmentVariable.Value,
                    IssueType = issueType
                };
                HealthStatuses.Add(variableStatus);
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

        public void ClearStatus()
        {
            HealthStatuses.Clear();
        }

        public List<EnvironmentVariableHealthStatus> HealthStatuses { get; }
    }
}