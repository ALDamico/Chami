using ChamiUI.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public ViewModelBase()
        {
            HasBeenChanged = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            HasBeenChanged = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool HasBeenChanged { get; protected set; }
    }
}