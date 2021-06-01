using System.Windows.Input;

namespace ChamiUI.PresentationLayer.Commands
{
    public static class MainWindowCommandsProvider 
    {
        static MainWindowCommandsProvider()
        {
            CancelEditingCommand = new RoutedCommand();
            RenameEnvironmentCommand = new RoutedCommand();
        }

        public static RoutedCommand CancelEditingCommand;
        public static RoutedCommand RenameEnvironmentCommand;
    }
}