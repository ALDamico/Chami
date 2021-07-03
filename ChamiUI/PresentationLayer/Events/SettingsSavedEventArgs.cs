using ChamiUI.PresentationLayer.ViewModels;
using System;

namespace ChamiUI.PresentationLayer.Events
{
    /// <summary>
    /// Subclass of <see cref="EventArgs"/>
    /// </summary>
    /// <seealso cref="EventArgs"/>
    public class SettingsSavedEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs a new <see cref="SettingsSavedEventArgs"/>.
        /// </summary>
        /// <param name="settings">The <see cref="SettingsViewModel"/> being saved.</param>
        public SettingsSavedEventArgs(SettingsViewModel settings)
        {
            Settings = settings;
        }
        
        /// <summary>
        /// The <see cref="SettingsViewModel"/> being saved.
        /// </summary>
        public SettingsViewModel Settings { get; }
    }
}