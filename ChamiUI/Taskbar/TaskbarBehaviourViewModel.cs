﻿using System.Windows.Input;
using ChamiUI.Taskbar.Commands;

namespace ChamiUI.Taskbar
{
    public class TaskbarBehaviourViewModel
    {
        public ICommand ShowWindowCommand
        {
            get => new ShowWindowCommand();
        }
    }
}