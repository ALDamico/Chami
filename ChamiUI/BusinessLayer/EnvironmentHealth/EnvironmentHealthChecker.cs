using System;
using System.Collections.Generic;
using System.Windows.Threading;
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

            _timer = new DispatcherTimer(TimeSpan.FromMilliseconds(configuration.CheckInterval), DispatcherPriority.Background, OnHealthTickElapsed, Dispatcher.CurrentDispatcher );
            _timer.Tick += OnHealthTickElapsed;
        }

        private void OnHealthTickElapsed(object sender, EventArgs e)
        {
            CheckEnvironment();
            RestartTimer();
        }

        private readonly EnvironmentHealthCheckerConfiguration _configuration;
        private readonly IEnvironmentHealthCheckerStrategy _checkStrategy;

        public double CheckEnvironment()
        {
            var result = _checkStrategy.CheckHealth(_environment, _configuration);
            HealthChecked?.Invoke(this, new HealthCheckedEventArgs(result, _checkStrategy.HealthStatuses));
            return result;
        }

        private EnvironmentViewModel _environment;

        public event EventHandler<HealthCheckedEventArgs> HealthChecked;

        public List<EnvironmentVariableHealthStatus> GetHealthStatusReport()
        {
            return _checkStrategy.HealthStatuses;
        }

        private readonly DispatcherTimer _timer;

        public void OnEnvironmentChanged(object sender, EnvironmentChangedEventArgs e)
        {
            _environment = e.NewActiveEnvironment;
            CheckEnvironment();
            RestartTimer();
        }

        private void RestartTimer()
        {
            _timer.Stop();
            _timer.Start();
        }

        public void DisableCheck()
        {
            _timer.Stop();
        }
    }
}