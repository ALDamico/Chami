using System.Windows;
using System.Windows.Input;

namespace ChamiUI.Windows.RunApplication;

public partial class RunApplicationWindow : Window
{
    public RunApplicationWindow()
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
}