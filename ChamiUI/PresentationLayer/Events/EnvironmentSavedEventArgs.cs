using ChamiUI.PresentationLayer.ViewModels;
using System;

namespace ChamiUI.PresentationLayer.Events
{
    /// <summary>
    /// Subclass of <see cref="EventArgs"/> used for handling the EnvironmentSaved event.
    /// </summary>
    /// <seealso cref="EventArgs"/>
    public class EnvironmentSavedEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs a new <see cref="EnvironmentSavedEventArgs"/> object.
        /// </summary>
        /// <param name="environmentViewModel">The <see cref="EnvironmentViewModel"/> that is being saved.</param>
        public EnvironmentSavedEventArgs(EnvironmentViewModel environmentViewModel)
        {
            EnvironmentViewModel = environmentViewModel;
            CheckEnvironmentExistence = true;
        }
        /// <summary>
        /// The <see cref="EnvironmentViewModel"/> that is being saved.
        /// </summary>
        public EnvironmentViewModel EnvironmentViewModel { get; }
        
        public bool CheckEnvironmentExistence { get; set; }
    }
}