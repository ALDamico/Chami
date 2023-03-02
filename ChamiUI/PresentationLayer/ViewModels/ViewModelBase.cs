using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ChamiUI.PresentationLayer.Events;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// Base abstract class for all viewmodels in Chami that implements the <see cref="INotifyPropertyChanged"/>
    /// interface.
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged"/>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notify the UI a property has changed
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler<MessageBoxTriggeredEventArgs> MessageBoxTriggered;

        protected virtual void ShowMessageBox(Action<MessageBoxResult> messageBoxAction, string messageBoxText, string messageBoxCaption, MessageBoxButton messageBoxButton = MessageBoxButton.OK, MessageBoxImage messageBoxImage = MessageBoxImage.None, MessageBoxResult defaultResult = MessageBoxResult.None, MessageBoxOptions messageBoxOptions = MessageBoxOptions.None)
        {
            if (MessageBoxTriggered == null)
            {
                return;
            }

            var eventArgs = new MessageBoxTriggeredEventArgs
            {
                MessageBoxAction = messageBoxAction,
                MessageBoxText = messageBoxText,
                MessageBoxCaption = messageBoxCaption,
                DefaultResult = defaultResult,
                MessageBoxButton = messageBoxButton,
                MessageBoxImage = messageBoxImage,
                MessageBoxOptions = messageBoxOptions
            };

            MessageBoxTriggered?.Invoke(this, eventArgs);
        }
    }
}