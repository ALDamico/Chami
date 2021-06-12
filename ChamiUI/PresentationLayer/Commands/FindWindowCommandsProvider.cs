using System.Windows.Input;

namespace ChamiUI.PresentationLayer.Commands
{
    public static class FindWindowCommandsProvider
    {
        public static RoutedCommand FindNextCommand;

        static FindWindowCommandsProvider()
        {
            FindNextCommand = new RoutedCommand();
        }
    }
}