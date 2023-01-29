using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;
using ChamiUI.Windows.MainWindow;

namespace ChamiUI.PresentationLayer.Utils;

public static class WindowUtils
{
    public static async Task CloseWindow(MainWindowViewModel mainWindowViewModel, MainWindow window)
    {
        window.SaveState();
        Application.Current.Shutdown(0);
        await Task.CompletedTask;
    }

    public static async Task PreventCloseWindow(MainWindowViewModel mainWindowViewModel, MainWindow window, CancelEventArgs cancelEventArgs)
    {
        MessageBox.Show(ChamiUIStrings.OperationInProgressCantCloseWindowMessage,
            ChamiUIStrings.OperationInProgressCantCloseWindowCaption, MessageBoxButton.OK, MessageBoxImage.Information);
        cancelEventArgs.Cancel = true;
        await Task.CompletedTask;
    }
}