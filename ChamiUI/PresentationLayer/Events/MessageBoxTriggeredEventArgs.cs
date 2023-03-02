using System;
using System.Windows;

namespace ChamiUI.PresentationLayer.Events;

public class MessageBoxTriggeredEventArgs : EventArgs
{
    public string MessageBoxText { get; init; }
    public string MessageBoxCaption { get; init; }
    public MessageBoxButton MessageBoxButton { get; init; }
    public MessageBoxImage MessageBoxImage { get; init; }
    public MessageBoxResult DefaultResult { get; init; }
    public MessageBoxOptions MessageBoxOptions { get; init; }
    public Action<MessageBoxResult> MessageBoxAction { get; init; }

    public void Show(Window owner = null)
    {
        var result = MessageBox.Show(owner!, MessageBoxText, MessageBoxCaption, MessageBoxButton, MessageBoxImage,
            DefaultResult, MessageBoxOptions);
        MessageBoxAction?.Invoke(result);
    }
}