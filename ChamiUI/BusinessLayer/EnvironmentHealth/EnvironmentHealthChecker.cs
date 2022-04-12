using System;
using System.Collections.Generic;
using ChamiUI.BusinessLayer.EnvironmentHealth.Strategies;
using ChamiUI.BusinessLayer.Validators;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.EnvironmentHealth
{
    public class EnvironmentHealthChecker
    {
        public EnvironmentHealthChecker(EnvironmentHealthCheckerConfiguration configuration, IEnvironmentHealthCheckerStrategy checkStrategy)
        {
            _configuration = configuration;
            _checkStrategy = checkStrategy;

            if (_checkStrategy == null)
            {
                throw new ArgumentNullException(nameof(checkStrategy),
                    ChamiUIStrings.NullHealthCheckStrategyExceptionMessage);
            }
        }

        private readonly EnvironmentHealthCheckerConfiguration _configuration;
        private readonly IEnvironmentHealthCheckerStrategy _checkStrategy;

        public double CheckEnvironment(EnvironmentViewModel environment)
        {
            var result = _checkStrategy.CheckHealth(environment, _configuration);
            HealthChecked?.Invoke(this, new HealthCheckedEventArgs(result));
            return result;
        }

        public event EventHandler<HealthCheckedEventArgs> HealthChecked;

        public List<EnvironmentVariableHealthStatus> GetHealthStatusReport()
        {
            return _checkStrategy.HealthStatuses;
        }
    }
}