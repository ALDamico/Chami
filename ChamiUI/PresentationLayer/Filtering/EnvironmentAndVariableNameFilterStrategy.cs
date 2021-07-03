using System;
using System.Windows.Data;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.PresentationLayer.Filtering
{
    /// <summary>
    /// An <see cref="IFilterStrategy"/> that filters based on environment names and environment variable names
    /// (but not values).
    /// </summary>
    /// <seealso cref="IFilterStrategy"/>
    public class EnvironmentAndVariableNameFilterStrategy:IFilterStrategy
    {
        /// <summary>
        /// Constructs a new <see cref="EnvironmentAndVariableNameFilterStrategy"/> object and sets its name.
        /// </summary>
        public EnvironmentAndVariableNameFilterStrategy()
        {
            Name = ChamiUIStrings.EnvironmentAndVariableNameFilterStrategyName;
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