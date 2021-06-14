using System;
using System.Windows.Data;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.PresentationLayer.Filtering
{
    public class EnvironmentAndVariableNameAndValueFilterStrategy:IFilterStrategy
    {
        public EnvironmentAndVariableNameAndValueFilterStrategy()
        {
            Name = ChamiUIStrings.EnvironmentAndVariableNameAndValueFilterStrategyName;
        }
        public string Name { get; }
        public string SearchedText { get; set; }
        public void OnFilter(object sender, FilterEventArgs args)
        {
            args.Accepted = false;
            if (args.Item is EnvironmentViewModel viewModel)
            {
                if (SearchedText == null)
                {
                    args.Accepted = true;
                    return;
                }
                else if (viewModel.Name.Contains(SearchedText, Comparison))
                {
                    args.Accepted = true;
                }
                foreach (var environmentVariable in viewModel.EnvironmentVariables)
                {
                    string variableName = environmentVariable.Name;
                    if (variableName.Contains(SearchedText, Comparison))
                    {
                        args.Accepted = true;
                    }

                    string variableValue = environmentVariable.Value;
                    if (variableValue.Contains(SearchedText, Comparison))
                    {
                        args.Accepted = true;
                    }
                }
            }
        }

        public StringComparison Comparison { get; set; }
    }
}