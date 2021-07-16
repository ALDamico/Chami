using System;
using System.Windows.Data;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.PresentationLayer.Filtering
{
    public class EnvironmentNameFilterStrategy : IFilterStrategy
    {
        /// <summary>
        /// Constructs a new <see cref="EnvironmentNameFilterStrategy"/> and sets its name.
        /// </summary>
        public EnvironmentNameFilterStrategy()
        {
            Name = ChamiUIStrings.EnvironmentNameFilterStrategyName;
        }

        public string Name { get; }

        public string SearchedText { get; set; }

        /// <summary>
        /// Filters a <see cref="CollectionViewSource"/>.
        /// An <see cref="EnvironmentViewModel"/> is accepted if it matches any of the following criteria:
        /// <list type="number">
        ///     <item>The <see cref="SearchedText"/> property is null (i.e., no filtering is being performed).</item>
        ///     <item>The <see cref="EnvironmentViewModel"/>'s Name property contains the <see cref="SearchedText"/>.</item>
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
                }
                else if (viewModel.Name.Contains(SearchedText, Comparison))
                {
                    args.Accepted = true;
                }
            }
        }

        public StringComparison Comparison { get; set; }
    }
}