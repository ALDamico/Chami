using System;

namespace ChamiUI.PresentationLayer.Events
{
    /// <summary>
    /// Subclass of <see cref="EventArgs"/> used by the EnvironmentExisting event.
    /// </summary>
    /// <seealso cref="EventArgs"/>
    public class EnvironmentExistingEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs a new <see cref="EnvironmentExistingEventArgs"/> object and sets its <see cref="Name"/> property.
        /// </summary>
        /// <param name="name"></param>
        public EnvironmentExistingEventArgs(string name)
        {
            Name = name;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public bool Exists
        {
            get => true;
        }
        public string Name { get; set; }
    }
}