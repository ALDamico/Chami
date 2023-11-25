using System;
using System.Media;
using System.Windows.Threading;
using Chami.CmdExecutor;
using Chami.CmdExecutor.Commands.Common;
using ChamiUI.Utils;
using ChamiUI.Windows.Exceptions;

namespace ChamiUI.BusinessLayer.Exceptions;

public static class ExceptionWindowFactory
{
    public static void ShowExceptionMessageBox(object sender, DispatcherUnhandledExceptionEventArgs args)
    {
        SystemSounds.Exclamation.Play();
        var exception = args.Exception;
        var exceptionWindow = new ExceptionWindow(exception);
        exceptionWindow.ShowDialog();
        /* if (Settings != null && Settings.LoggingSettings.LoggingEnabled)
            {
                Log.Logger.Error("{Message}", exception.Message);
                Log.Logger.Error("{Message}", args.Exception.StackTrace);
            }*/

        if (exceptionWindow.IsApplicationTerminationRequested)
        {
            if (exceptionWindow.IsApplicationRestartRequested)
            {
                IShellCommand restartCommand = new OpenInExplorerCommand(AppUtils.GetApplicationExecutablePath());
                restartCommand.Execute();
            }

            Environment.Exit(-1);
        }
#if !DEBUG
        args.Handled = true; // TODO react to user choice
#else
            throw exception;
#endif
    }
}