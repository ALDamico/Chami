using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace ChamiUI.Windows.RunApplication;

public partial class RunApplicationWindow : Window
{
    private RunApplicationWindow()
    {
        InitializeComponent();
    }

    private void CloseCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void CloseCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        Close();
    }

    private static RunApplicationWindow _instance;

    public static RunApplicationWindow Instance
    {
        get
        {
            _instance ??= new RunApplicationWindow();

            _instance.Visibility = Visibility.Visible;
            _instance.Focus();

            return _instance;
        }
    }

    private void RunApplicationWindow_OnClosing(object sender, CancelEventArgs e)
    {
        e.Cancel = true;
        Hide();
    }
}