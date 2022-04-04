using System;

namespace ChamiUI.PresentationLayer.Events
{
    public class HealthCheckedEventArgs: EventArgs
    {
        public HealthCheckedEventArgs(double health)
        {
            Health = health;
        }
        public double Health { get; }
    }
}