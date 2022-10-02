using System;
using System.Windows;

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
}