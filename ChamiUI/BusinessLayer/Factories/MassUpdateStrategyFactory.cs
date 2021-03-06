using System;
using System.Collections.Generic;
using ChamiUI.BusinessLayer.MassUpdateStrategies;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Factories
{
    public static class MassUpdateStrategyFactory
    {
        public static IMassUpdateStrategy GetMassUpdateStrategyByViewModel(MassUpdateStrategyViewModel viewModel, string variableName, string variableValue, IEnumerable<EnvironmentViewModel> environments)
        {
            if (viewModel.Name == ChamiUIStrings.MassUpdateStrategyName_UpdateAll)
            {
                return new UpdateAllStrategy(variableName, variableValue);
            }

            return new UpdateSelectedStrategy(variableName, variableValue, environments);
        }
    }
}