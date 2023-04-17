using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using AsyncAwaitBestPractices.MVVM;
using ChamiUI.BusinessLayer.Annotations;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.Utils;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// Base abstract class for all viewmodels in Chami that implements the <see cref="INotifyPropertyChanged"/>
    /// interface.
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged"/>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected ViewModelBase()
        {
            CloseCommand = new AsyncCommand<Window>(ExecuteCloseWindow);
        }
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

        /// <summary>
        /// Show a message box in a MVVM-friendly way. This method can be overridden in derived classes.
        /// </summary>
        /// <param name="messageBoxAction">What action to take when the message box is closed or otherwise dismissed.</param>
        /// <param name="messageBoxText">The text to show inside the message box.</param>
        /// <param name="messageBoxCaption">The text to show in the message box's title bar.</param>
        /// <param name="messageBoxButton">What buttons to show in the message box.</param>
        /// <param name="messageBoxImage">The icon to show inside the message box.</param>
        /// <param name="defaultResult">The default result when the message box is dismissed.</param>
        /// <param name="messageBoxOptions">Additional configuration options for the message box.</param>
        protected virtual void ShowMessageBox(Action<MessageBoxResult> messageBoxAction, string messageBoxText, string messageBoxCaption, MessageBoxButton messageBoxButton = MessageBoxButton.OK, MessageBoxImage messageBoxImage = MessageBoxImage.None, MessageBoxResult defaultResult = MessageBoxResult.None, MessageBoxOptions messageBoxOptions = MessageBoxOptions.None)
        {
            if (MessageBoxTriggered == null)
            {
                var window = AppUtils.GetMainWindow();
                MessageBoxTriggered += window.ShowMessageBox;
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
        
        public IAsyncCommand<Window> CloseCommand { get; protected set; }
        
        protected async Task ExecuteCloseWindow(Window arg)
        {
            arg?.Close();

            await Task.CompletedTask;
        }

        private bool _isBusy;
        private string _busyMessage;

        protected virtual void SetBusyState(bool isBusy, string message = null)
        {
            IsBusy = isBusy;
            BusyMessage = message;
        }

        [NonPersistentSetting]
        public string BusyMessage
        {
            get => _busyMessage;
            set
            {
                _busyMessage = value;
                OnPropertyChanged();
            }
        }

        [NonPersistentSetting]
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }
    }
}