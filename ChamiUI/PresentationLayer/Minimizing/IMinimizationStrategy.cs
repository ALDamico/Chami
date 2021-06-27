using System;
using System.Windows;


namespace ChamiUI.PresentationLayer.Minimizing
{
    /// <summary>
    /// Interface that describes how a <see cref="Window"/> should behave when minimized.
    /// </summary>
    public interface IMinimizationStrategy
    {
        /// <summary>
        /// Execute the action of minimizing the window.
        /// </summary>
        /// <param name="window">The <see cref="Window"/> object to apply the minimization to.</param>
        /// <param name="action">Additional code to execute when minimizing the <see cref="Window"/></param>
        void Minimize(Window window, Action action);

        /// <summary>
        /// The localizable name for this strategy.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Singleton instance of the class
        /// </summary>
        static IMinimizationStrategy Instance { get; }
    }
}