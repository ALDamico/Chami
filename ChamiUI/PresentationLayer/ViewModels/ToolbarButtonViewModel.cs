using System.Windows.Media;
using AsyncAwaitBestPractices.MVVM;
using MahApps.Metro.IconPacks;

namespace ChamiUI.PresentationLayer.ViewModels;

public class ToolbarButtonViewModel : ViewModelBase
{
    private string _caption;
    private PackIconFontAwesomeKind _icon;
    private string _toolTip;
    private string _commandNameName;
    private SolidColorBrush _foregroundColor;

    public string Caption
    {
        get => _caption;
        set
        {
            _caption = value;
            OnPropertyChanged();
        }
    }

    public PackIconFontAwesomeKind Icon
    {
        get => _icon;
        set
        {
            _icon = value;
            OnPropertyChanged();
        }
    }

    public string ToolTip
    {
        get => _toolTip;
        set
        {
            _toolTip = value;
            OnPropertyChanged();
        }
    }

    public string CommandName
    {
        get => _commandNameName;
        set
        {
            _commandNameName = value;
            OnPropertyChanged();
        }
    }

    public SolidColorBrush ForegroundColor
    {
        get => _foregroundColor;
        set
        {
            _foregroundColor = value;
            OnPropertyChanged();
        }
    }
}