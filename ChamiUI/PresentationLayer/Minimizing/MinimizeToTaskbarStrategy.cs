using System;
using System.Windows;
using ChamiUI.Localization;

namespace ChamiUI.PresentationLayer.Minimizing
{
    /// <summary>
    /// Represents the act of minimizing a window to the task bar.
    /// It's for all intents and purposes a NOP.
    /// </summary>
    public class MinimizeToTaskbarStrategy : IMinimizationStrategy
    {
        /// <summary>
        /// Private constructor to forbid creating new instances explicitly.
        /// </summary>
        private MinimizeToTaskbarStrategy()
        {
        }

        /// <summary>
        /// Minimize the window to the taskbar.
        /// </summary>
        /// <param name="window">The <see cref="Window"/> object to minimize.</param>
        /// <param name="action">This parameter is ignored in this implementation.</param>
        public void Minimize(Window window, Action action)
        {
            //NOP
        }

        /// <summary>
        /// Localizable name.
        /// </summary>
        public string Name => ChamiUIStrings.MinimizeToTaskbarStrategyName;
        /// <summary>
        /// Singleton instance.
        /// </summary>
        private static IMinimizationStrategy _instance;

        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static IMinimizationStrategy Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MinimizeToTaskbarStrategy();
                }

                return _instance;
            }
        }
    }
}