using System;
using System.Windows;
using ChamiUI.Localization;

namespace ChamiUI.PresentationLayer.Minimizing
{
    /// <summary>
    /// Represents the act of minimizing the window to the system tray.
    /// </summary>
    public class MinimizeToTrayStrategy : IMinimizationStrategy
    {
        /// <summary>
        /// Perform the action of minimizing the window by hiding it.
        /// </summary>
        /// <param name="window">The <see cref="Window"/> object to apply the minimizationn to.</param>
        /// <param name="action">Additional code to execute when minimizing.</param>
        public void Minimize(Window window, Action action)
        {
            if (action != null)
            {
                action.Invoke();
            }

            window.Hide();
        }

        /// <summary>
        /// Singleton instance.
        /// </summary>
        private static IMinimizationStrategy _instance;

        /// <summary>
        /// Localizable name
        /// </summary>
        public string Name => ChamiUIStrings.MinimizeToTrayStrategyName;

        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static IMinimizationStrategy Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MinimizeToTrayStrategy();
                }

                return _instance;
            }
        }
    }
}