using System.Windows.Input;

namespace ChamiUI.PresentationLayer.Commands
{
    /// <summary>
    /// Helper class that provides <see cref="RoutedCommand"/>s for the main window of the application.
    /// </summary>
    public static class MainWindowCommandsProvider
    {
        public static readonly RoutedCommand CancelEditingCommand = new RoutedCommand();
        public static readonly RoutedCommand RenameEnvironmentCommand = new RoutedCommand();
        public static readonly RoutedCommand FocusFilterTextbox = new RoutedCommand();
        public static readonly RoutedCommand CreateTemplateEnvironmentCommand = new RoutedCommand();
        public static readonly RoutedCommand EditEnvironmentCommand = new RoutedCommand();
        public static readonly RoutedCommand DeleteEnvironmentCommand = new RoutedCommand();
        public static readonly RoutedCommand DuplicateEnvironmentCommand = new RoutedCommand();
    }
}