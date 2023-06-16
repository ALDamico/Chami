using System.Windows;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Windows.Abstract;

public abstract class ChamiWindow : Window
{
    protected ChamiWindow()
    {
        DataContextChanged += OnDataContextChanged;
    }

    protected virtual void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is ViewModelBase viewModelBase)
        {
            viewModelBase.MessageBoxTriggered += ShowMessageBox;
        }
    }

    protected internal virtual void ShowMessageBox(object sender, MessageBoxTriggeredEventArgs e)
    {
        e.Show(this);
    }
}