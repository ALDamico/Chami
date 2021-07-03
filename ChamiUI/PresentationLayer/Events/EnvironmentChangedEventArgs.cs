using System;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.PresentationLayer.Events
{
    /// <summary>
    /// Subclass of <see cref="EventArgs"/> that contains information about the changing of the active environment in
    /// the Chami application.
    /// </summary>
    /// <seealso cref="EventArgs"/>
    public class EnvironmentChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs a new <see cref="EnvironmentChangedEventArgs"/> object.
        /// </summary>
        /// <param name="newActiveEnvironment">The new active environment.</param>
        public EnvironmentChangedEventArgs(EnvironmentViewModel newActiveEnvironment)
        {
            NewActiveEnvironment = newActiveEnvironment;
            if (NewActiveEnvironment != null)
            {
                NewActiveEnvironment.IsActive = true;
            }
        }
        /// <summary>
        /// The new active environment.
        /// </summary>
        public EnvironmentViewModel NewActiveEnvironment { get; }
    }
}