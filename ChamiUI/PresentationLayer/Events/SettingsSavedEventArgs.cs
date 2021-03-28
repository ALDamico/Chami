using ChamiUI.PresentationLayer.ViewModels;
using System;

namespace ChamiUI.PresentationLayer.Events
{
    public class SettingsSavedEventArgs : EventArgs
    {
        public SettingsSavedEventArgs(SettingsViewModel settings)
        {
            Settings = settings;
        }
        public SettingsViewModel Settings { get; }
    }
}