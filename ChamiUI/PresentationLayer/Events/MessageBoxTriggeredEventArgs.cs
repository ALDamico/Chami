using System;
using System.Windows;

namespace ChamiUI.PresentationLayer.Events;

public class MessageBoxTriggeredEventArgs : EventArgs
{
    /// <summary>
    /// The text to show inside the message box.
    /// </summary>
    public string MessageBoxText { get; init; }
    /// <summary>
    /// The text to show in the message box's title bar.
    /// </summary>
    public string MessageBoxCaption { get; init; }
    /// <summary>
    /// What buttons to show in the message box.
    /// </summary>
    public MessageBoxButton MessageBoxButton { get; init; }
    /// <summary>
    /// The icon to show inside the message box.
    /// </summary>
    public MessageBoxImage MessageBoxImage { get; init; }
    /// <summary>
    /// The default result when the message box is dismissed.
    /// </summary>
    public MessageBoxResult DefaultResult { get; init; }
    /// <summary>
    /// Additional configuration options for the message box.
    /// </summary>
    public MessageBoxOptions MessageBoxOptions { get; init; }
    /// <summary>
    /// What action to take when the message box is closed or otherwise dismissed.
    /// </summary>
    public Action<MessageBoxResult> MessageBoxAction { get; init; }

    /// <summary>
    /// Show the message box using the supplied configuration.
    /// </summary>
    /// <param name="owner">The window that owns this message box.</param>
    public void Show(Window owner = null)
    {
        var result = MessageBox.Show(owner!, MessageBoxText, MessageBoxCaption, MessageBoxButton, MessageBoxImage,
            DefaultResult, MessageBoxOptions);
        MessageBoxAction?.Invoke(result);
    }
}