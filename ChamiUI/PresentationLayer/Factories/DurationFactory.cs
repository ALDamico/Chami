using System;
using System.Windows;

namespace ChamiUI.PresentationLayer.Factories
{
    public static class DurationFactory
    {
        public static Duration FromMilliseconds(int milliseconds)
        {
            var timespan = TimeSpan.FromMilliseconds(milliseconds);
            return new Duration(timespan);
        }
    }
}