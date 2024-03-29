﻿using System;
using System.Windows;
using System.Windows.Input;
using ChamiUI.Localization;
using ChamiUI.Windows.MainWindow;
using Microsoft.Extensions.DependencyInjection;

namespace ChamiUI.Taskbar.Commands
{
    public class ShowWindowCommand:ICommand
    {
        public bool CanExecute(object parameter)
        {
            var mainWindow = Application.Current.MainWindow;
            return mainWindow == null ||
                   mainWindow.Visibility.Equals(Visibility.Hidden);
        }

        public void Execute(object parameter)
        {
            var mainWindow = (Application.Current as App)?.ServiceProvider.GetRequiredService<MainWindow>();
            Application.Current.MainWindow = mainWindow;
            if (mainWindow == null)
            {
                throw new InvalidOperationException(ChamiUIStrings.MainWindowInitError);
            }
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