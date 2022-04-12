using System.Collections.Generic;
using Chami.Db.Entities;
using ChamiUI.BusinessLayer.Validators;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.EnvironmentHealth.Strategies
{
    public interface IEnvironmentHealthCheckerStrategy
    {
        double CheckHealth(EnvironmentViewModel environment, EnvironmentHealthCheckerConfiguration configuration);
        void ClearStatus();
        List<EnvironmentVariableHealthStatus> HealthStatuses { get; }
    }
}