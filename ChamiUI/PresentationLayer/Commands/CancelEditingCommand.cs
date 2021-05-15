using System;
using System.Windows.Input;

namespace ChamiUI.PresentationLayer.Commands
{
    public static class CancelEditingCommandProvider 
    {
        static CancelEditingCommandProvider()
        {
            CancelEditingCommand = new RoutedCommand();
        }

        public static RoutedCommand CancelEditingCommand;
    }
}