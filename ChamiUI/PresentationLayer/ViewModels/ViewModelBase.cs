using System.ComponentModel;
using System.Runtime.CompilerServices;

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
    }
}