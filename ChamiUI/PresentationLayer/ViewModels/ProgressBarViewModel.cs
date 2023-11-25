using System.Windows.Media;
using ChamiUI.PresentationLayer.Utils;

namespace ChamiUI.PresentationLayer.ViewModels;

public class ProgressBarViewModel : ViewModelBase
{
    private double _minumum;
    private double _maximum;
    private double _value;
    private Brush _foreground;

    public double Minimum
    {
        get => _minumum;
        set
        {
            _minumum = value;
            OnPropertyChanged();
        }
    }

    public double Maximum
    {
        get => _maximum;
        set
        {
            _maximum = value;
            OnPropertyChanged();
        }
    }

    public double Value
    {
        get => _value;
        set
        {
            _value = value;
            OnPropertyChanged();
        }
    }

    public Brush Foreground
    {
        get => _foreground;
        set
        {
            _foreground = value;
            OnPropertyChanged();
        }
    }

    public void Reset()
    {
        Value = 0;
        Foreground = ResourceUtils.DefaultProgressBarColor;
    }
}