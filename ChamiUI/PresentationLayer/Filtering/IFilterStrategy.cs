using System;
using System.Windows.Data;

namespace ChamiUI.PresentationLayer.Filtering
{
    /// <summary>
    /// Interface that describes a filtering strategy.
    /// </summary>
    public interface IFilterStrategy
    {
        /// <summary>
        /// A friendly name of the filter strategy for use by the UI.
        /// </summary>
        string Name { get;  }
        
        /// <summary>
        /// The text to search for.
        /// </summary>
        string SearchedText { get; set; }
        
        /// <summary>
        /// A method that reacts to the Filter event.
        /// </summary>
        /// <param name="sender">The object that initiated the event.</param>
        /// <param name="args">Information about the Filter event.</param>
        void OnFilter(object sender, FilterEventArgs args);
        
        /// <summary>
        /// The <see cref="StringComparison"/> to use when filtering.
        /// </summary>
        StringComparison Comparison { get; set; }
    }
}