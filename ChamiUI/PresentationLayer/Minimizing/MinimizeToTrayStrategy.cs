using System;
using System.Windows;
using ChamiUI.Localization;

namespace ChamiUI.PresentationLayer.Minimizing
{
    public class MinimizeToTrayStrategy: IMinimizationStrategy
    {
        public void Minimize(Window window, Action action)
        {
            if (action != null)
            {
                action.Invoke();
            }
            
            window.Hide();
        }

        private static IMinimizationStrategy _instance;

        public string Name => ChamiUIStrings.MinimizeToTrayStrategyName;
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