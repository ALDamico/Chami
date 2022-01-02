using System.Windows.Controls;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class ToolbarInfoViewModel : ViewModelBase
    {
        public ToolbarInfoViewModel(ToolBar toolBar)
        {
            ToolBar = toolBar;
        }
        public ToolBar ToolBar { get; internal set; }
        private string _toolbarName;
        private string _toolbarFriendlyName;
        private bool _isVisible;
        private int _bandOccupied;
        private int _ordinalPositionInBand;

        public int OrdinalPositionInBand
        {
            get => _ordinalPositionInBand;
            set
            {
                _ordinalPositionInBand = value;
                OnPropertyChanged(nameof(OrdinalPositionInBand));
            }
        }

        public int BandOccupied
        {
            get => _bandOccupied;
            set
            {
                _bandOccupied = value;
                OnPropertyChanged(nameof(BandOccupied));
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }

        public string ToolbarFriendlyName
        {
            get => _toolbarFriendlyName;
            set
            {
                _toolbarFriendlyName = value;
                OnPropertyChanged(nameof(ToolbarFriendlyName));
            }
        }
        

        public string ToolbarName
        {
            get => _toolbarName;
            set
            {
                _toolbarName = value;
                OnPropertyChanged(nameof(ToolbarName));
            }
        }
    }
}