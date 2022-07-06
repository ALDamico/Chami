using System;
using System.Collections.Generic;
using ChamiUI.BusinessLayer.Validators;

namespace ChamiUI.PresentationLayer.Events
{
    public class HealthCheckedEventArgs: EventArgs
    {
        public HealthCheckedEventArgs(double health, List<EnvironmentVariableHealthStatus> healthStatusList)
        {
            Health = health;
            HealthStatusList = healthStatusList;
        }
        public double Health { get; }
        public List<EnvironmentVariableHealthStatus> HealthStatusList { get; }
    }
}