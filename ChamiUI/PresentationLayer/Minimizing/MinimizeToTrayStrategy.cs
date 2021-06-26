using System;
using System.Windows;

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
    }
}