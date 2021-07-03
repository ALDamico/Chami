using System;

namespace ChamiUI.PresentationLayer.Events
{
    /// <summary>
    /// Subclass of <see cref="EventArgs"/> used for handling events of environments changing their name.
    /// </summary>
    /// <seealso cref="EventArgs"/>
    public class EnvironmentRenamedEventArgs:EventArgs
    {
        /// <summary>
        /// Constructs a new <see cref="EnvironmentRenamedEventArgs"/>.
        /// </summary>
        /// <param name="newName">The new name of the environment.</param>
        public EnvironmentRenamedEventArgs(string newName)
        {
            NewName = newName;
        }
        
        /// <summary>
        /// The new name of the environment.
        /// </summary>
        public string NewName { get; }
        
    }
}