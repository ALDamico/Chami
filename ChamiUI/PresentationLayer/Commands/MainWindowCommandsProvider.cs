using System.Windows.Input;

namespace ChamiUI.PresentationLayer.Commands
{
    /// <summary>
    /// Helper class that provides <see cref="RoutedCommand"/>s for the main window of the application.
    /// </summary>
    public static class MainWindowCommandsProvider 
    {
        static MainWindowCommandsProvider()
        {
            CancelEditingCommand = new RoutedCommand();
            RenameEnvironmentCommand = new RoutedCommand();
            FocusFilterTextbox = new RoutedCommand();
            CreateTemplateEnvironmentCommand = new RoutedCommand();
            EditEnvironmentCommand = new RoutedCommand();
            DeleteEnvironmentCommand = new RoutedCommand();
            DuplicateEnvironmentCommand = new RoutedCommand();
        }

        public static RoutedCommand CancelEditingCommand;
        public static RoutedCommand RenameEnvironmentCommand;
        public static RoutedCommand FocusFilterTextbox;
        public static RoutedCommand CreateTemplateEnvironmentCommand;
        public static RoutedCommand EditEnvironmentCommand;
        public static RoutedCommand DeleteEnvironmentCommand;
        public static RoutedCommand DuplicateEnvironmentCommand;
    }
}