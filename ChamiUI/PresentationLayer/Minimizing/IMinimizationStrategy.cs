using System;
using System.Windows;

namespace ChamiUI.PresentationLayer.Minimizing
{
    public interface IMinimizationStrategy
    {
        void Minimize(Window window, Action action);
        string Name { get; }

        static IMinimizationStrategy Instance { get; }
    }
}