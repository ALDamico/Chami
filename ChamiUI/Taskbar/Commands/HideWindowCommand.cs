﻿using System;
using System.Windows;
using System.Windows.Input;
using ChamiUI.Windows.MainWindow;

namespace ChamiUI.Taskbar.Commands
{
    public class HideWindowCommand:ICommand
    {
        public bool CanExecute(object parameter)
        {
            return Application.Current.MainWindow != null &&
                   Application.Current.MainWindow.Visibility.Equals(Visibility.Visible);
        }

        public void Execute(object parameter)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.SaveState();
                mainWindow.Hide();
            }
                
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested += value;
        }
    }
}