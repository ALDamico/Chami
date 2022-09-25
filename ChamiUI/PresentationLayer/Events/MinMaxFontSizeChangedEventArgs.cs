using System;

namespace ChamiUI.PresentationLayer.Events;

public class MinMaxFontSizeChangedEventArgs : EventArgs
{
    public MinMaxFontSizeChangedEventArgs(double? minFontSize, double? maxFontSize)
    {
        MinFontSize = minFontSize;
        MaxFontSize = maxFontSize;
    }
    public double? MinFontSize { get; }
    public double? MaxFontSize { get; }
}