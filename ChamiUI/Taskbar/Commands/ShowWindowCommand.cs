using System;
using System.Windows;
using System.Windows.Input;
using ChamiUI.Windows.MainWindow;

namespace ChamiUI.Taskbar.Commands
{
    public class ShowWindowCommand:ICommand
    {
        public bool CanExecute(object parameter)
        {
            return Application.Current.MainWindow == null ||
                   Application.Current.MainWindow.Visibility.Equals(Visibility.Hidden);
        }

        public void Execute(object parameter)
        {
            var mainWindow = new MainWindow();
            Application.Current.MainWindow = mainWindow;
            mainWindow.ResumeState();
            mainWindow.Show();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}