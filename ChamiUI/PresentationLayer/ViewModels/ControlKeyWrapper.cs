using System;
using System.Windows.Controls;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class ControlKeyWrapper
    {
        public ControlKeyWrapper(string controlDisplayName, UserControl control)
        {
            Guid = Guid.NewGuid();
            ControlDisplayName = controlDisplayName;
            Control = control;
        }
        public Guid Guid { get; }
        public string ControlDisplayName { get; }
        public UserControl Control { get; }
    }
}