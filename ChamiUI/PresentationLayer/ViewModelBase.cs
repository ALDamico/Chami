using System.ComponentModel;
using System.Runtime.CompilerServices;
using ChamiUI.Annotations;

namespace ChamiUI.PresentationLayer
{
    public class ViewModelBase:INotifyPropertyChanged
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