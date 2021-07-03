using System;
using System.Windows;

namespace ChamiUI.PresentationLayer.Factories
{
    /// <summary>
    /// Helper class that is used to constructs new <see cref="Duration"/> objects from an integer expressing its
    /// duration in milliseconds.
    /// </summary>
    public static class DurationFactory
    {
        /// <summary>
        /// Constructs a new <see cref="Duration"/> object.
        /// </summary>
        /// <param name="milliseconds">The number of milliseconds this duration should last.</param>
        /// <returns>A new <see cref="Duration"/> object that lasts the amount of milliseconds specified.</returns>
        public static Duration FromMilliseconds(int milliseconds)
        {
            var timespan = TimeSpan.FromMilliseconds(milliseconds);
            return new Duration(timespan);
        }
    }
}