using System;
using System.Windows.Data;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.PresentationLayer.Filtering
{
    public class EnvironmentAndVariableNameFilterStrategy:IFilterStrategy
    {
        public EnvironmentAndVariableNameFilterStrategy()
        {
            Name = ChamiUIStrings.EnvironmentAndVariableNameFilterStrategyName;
        }
        public string Name { get; set; }
        public string SearchedText { get; set; }
        public void OnFilter(object sender, FilterEventArgs args)
        {
            args.Accepted = false;
            if (args.Item is EnvironmentViewModel viewModel)
            {
                if (viewModel.Name.Contains(SearchedText, Comparison))
                {
                    args.Accepted = true;
                }

                foreach (var environmentVariable in viewModel.EnvironmentVariables)
                {
                    if (environmentVariable.Name.Contains(SearchedText, Comparison))
                    {
                        args.Accepted = true;
                    }
                }
            }
        }

        public StringComparison Comparison { get; set; }
    }
}