using System.Windows.Media;

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

    public Brush Foregound
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
        Foregound = Brushes.Green;
    }
}