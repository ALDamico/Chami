using System;
using System.Windows.Data;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.PresentationLayer.Filtering
{
    /// <summary>
    /// An <see cref="IFilterStrategy"/> that filters based on environment names and environment variable names and
    /// values.
    /// </summary>
    /// <seealso cref="IFilterStrategy"/>
    public class EnvironmentAndVariableNameAndValueFilterStrategy : IFilterStrategy
    {
        /// <summary>
        /// Constructs a new <see cref="EnvironmentAndVariableNameAndValueFilterStrategy"/> object and sets its
        /// <see cref="Name"/> property.
        /// </summary>
        public EnvironmentAndVariableNameAndValueFilterStrategy()
        {
            Name = ChamiUIStrings.EnvironmentAndVariableNameAndValueFilterStrategyName;
        }

        public string Name { get; }
        public string SearchedText { get; set; }

        /// <summary>
        /// Filters a <see cref="CollectionViewSource"/>.
        /// An <see cref="EnvironmentViewModel"/> is accepted if it matches any of the following criteria:
        /// <list type="number">
        ///     <item>The <see cref="SearchedText"/> property is null (i.e., no filtering is being performed).</item>
        ///     <item>The <see cref="EnvironmentViewModel"/>'s Name property contains the <see cref="SearchedText"/>.</item>
        ///     <item>Any of the <see cref="EnvironmentVariableViewModel"/>s' Name property in the environment
        ///     contains the <see cref="SearchedText"/>.</item>
        ///     <item>Any of the <see cref="EnvironmentVariableViewModel"/>s' Value property in the environment
        ///     contains the <see cref="SearchedText"/>.</item>
        /// </list>
        /// </summary>
        /// <param name="sender">The object that initiated the Filter event.</param>
        /// <param name="args">Determines if an item is accepted or rejected by the Filter event.</param>
        /// <seealso cref="CollectionViewSource"/>
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

                if (viewModel.Name.Contains(SearchedText, Comparison))
                {
                    args.Accepted = true;
                    return;
                }

                foreach (var environmentVariable in viewModel.EnvironmentVariables)
                {
                    string variableName = environmentVariable.Name;
                    if (variableName.Contains(SearchedText, Comparison))
                    {
                        args.Accepted = true;
                        return;
                    }

                    string variableValue = environmentVariable.Value;
                    if (variableValue.Contains(SearchedText, Comparison))
                    {
                        args.Accepted = true;
                        return;
                    }
                }
            }
        }

        public StringComparison Comparison { get; set; }
    }
}