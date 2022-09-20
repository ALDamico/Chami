using System;
using System.Diagnostics;
using System.Windows;

namespace ChamiUI.Interop;

internal static class User32Utils
{
    internal static bool IsIconic(Process process)
    {
        return User32Api.IsIconic(process.MainWindowHandle);
    }

    internal static bool SetForegroundWindow(Process process)
    {
        return User32Api.SetForegroundWindow(process.MainWindowHandle);
    }

    internal static bool RestoreWindow(Process process)
    {
        return User32Api.ShowWindow(process.MainWindowHandle, User32Api.SW_RESTORE);
    }

    internal static void FocusOtherWindow(Process process)
    {
        if (IsIconic(process))
        {
            RestoreWindow(process);
        }

        SetForegroundWindow(process);
    }

    internal static void FocusOtherWindowAndExit(Process process)
    {
        FocusOtherWindow(process);
        Environment.Exit(0);
    }
}