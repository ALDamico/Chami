using System.ComponentModel;
using ChamiUI.BusinessLayer.Annotations;
using ChamiUI.PresentationLayer.Filtering;
using ChamiUI.PresentationLayer.Minimizing;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// Contains the main window state that can be saved to the database and restored when the application is reopened
    /// or the main window is hidden and shown again.
    /// </summary>
    [ExplicitSaveOnly]
    public class MainWindowSavedBehaviourViewModel : SettingCategoryViewModelBase
    {

        /// <summary>
        /// Determines if the filtering is performed in a case-sensitive manner or not.
        /// </summary>
        public bool IsCaseSensitiveSearch { get; set; }

        /// <summary>
        /// The height of the main window.
        /// </summary>
        public double Height { get; set; }
        
        /// <summary>
        /// The width of the main window.
        /// </summary>
        public double Width { get; set; }
        
        /// <summary>
        /// The position on the left coordinate of the screen of the main window.
        /// </summary>
        public double XPosition { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public double YPosition { get; set; }
        public IFilterStrategy SearchPath { get; set; }
        public SortDescription SortDescription { get; set; }
        public IMinimizationStrategy MinimizationStrategy { get; set; }
    }
}