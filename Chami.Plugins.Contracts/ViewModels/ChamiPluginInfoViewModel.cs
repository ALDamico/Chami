using System;

namespace Chami.Plugins.Contracts.ViewModels
{
    public class ChamiPluginInfoViewModel : ViewModelBase
    {
        private string _author;
        private string _version;
        private DateTime _creationDate;
        private string _pluginName;
        private bool _isEnabled;

        public string Author
        {
            get => _author;
            set
            {
                _author = value;
                OnPropertyChanged(nameof(Author));
            }
        }

        public string Version
        {
            get => _version;
            set
            {
                _version = value;
                OnPropertyChanged(nameof(Version));
            }
        }

        public DateTime CreationDate
        {
            get => _creationDate;
            set
            {
                _creationDate = value;
                OnPropertyChanged(nameof(CreationDate));
            }
        }

        public string PluginName
        {
            get => _pluginName;
            set
            {
                _pluginName = value;
                OnPropertyChanged(nameof(PluginName));
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
            }
        }
    }
}