using System;
using System.Windows;
using System.Windows.Input;
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

    public static RoutedCommand TerminateApplicationCommand = new RoutedCommand();

    private void TerminateApplicationCommand_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void TerminateApplicationCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        IsApplicationTerminationRequested = true;
        Close();
    }
}