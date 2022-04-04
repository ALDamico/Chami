using Chami.Db.Entities;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.EnvironmentHealth.Strategies
{
    public interface IEnvironmentHealthCheckerStrategy
    {
        double CheckHealth(EnvironmentViewModel environment, EnvironmentHealthCheckerConfiguration configuration);
    }
}