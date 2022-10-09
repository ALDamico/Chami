using System;
using System.Windows;
using System.Windows.Input;
using Chami.Db.Utils;
using ChamiUI.BusinessLayer.Exceptions.Exporter;
using ChamiUI.BusinessLayer.Factories;
using ChamiUI.Utils;
using Microsoft.Win32;
using Serilog;

namespace ChamiUI.Windows.Exceptions;

public partial class ExceptionWindow : Window
{
    public ExceptionWindow(Exception exception)
    {
        Exception = exception;
        InitializeComponent();
    }
    
    public static readonly DependencyProperty ExceptionProperty = DependencyProperty.Register("Exception", typeof(Exception), typeof(ExceptionWindow));

    public Exception Exception
    {
        get => (Exception) GetValue(ExceptionProperty);
        set => SetValue(ExceptionProperty, value);
    }

    public string ExceptionType => Exception?.GetType().Name;
    public bool IsApplicationTerminationRequested { get; private set; }
    public bool IsApplicationRestartRequested { get; private set; }

    private void CloseCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void CloseCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        Close();
    }

    public static readonly RoutedCommand TerminateApplicationCommand = new RoutedCommand();
    public static readonly RoutedCommand WriteExceptionDetailsCommand = new RoutedCommand();

    private void TerminateApplicationCommand_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void TerminateApplicationCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        IsApplicationTerminationRequested = true;

        if (RestartRequestedParameter.Equals(e.Parameter))
        {
            IsApplicationRestartRequested = true;
        }
        
        Close();
    }

    private const string RestartRequestedParameter = "restartRequested";

    private void WriteExceptionDetailsCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void WriteExceptionDetailsCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        var saveFileDialog = new SaveFileDialog();
        saveFileDialog.AddExtension = true;
        saveFileDialog.OverwritePrompt = true;
        saveFileDialog.DefaultExt = "*.log";
        var result = saveFileDialog.ShowDialog();

        if (result == true)
        {
            IExceptionExporter exporter = new ExceptionExporter(saveFileDialog.FileName);
            exporter.Export(Exception, AppUtils.GetLogFilePath());
        }
    }
}