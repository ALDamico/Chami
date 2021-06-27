using System;
using System.Windows;
using ChamiUI.Localization;

namespace ChamiUI.PresentationLayer.Minimizing
{
    public class MinimizeToTaskbarStrategy : IMinimizationStrategy
    {
        private MinimizeToTaskbarStrategy()
        {
            
        }
        public void Minimize(Window window, Action action)
        {
            //NOP
        }

        public string Name => ChamiUIStrings.MinimizeToTaskbarStrategyName;
        private static IMinimizationStrategy _instance;

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