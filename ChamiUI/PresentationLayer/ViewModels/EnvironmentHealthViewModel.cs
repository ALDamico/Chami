using System;
using System.Windows.Media;
using Brush = System.Drawing.Brush;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class EnvironmentHealthViewModel : ViewModelBase
    {
        public double HealthIndex
        {
            get => _healthIndex;
            set
            {
                _healthIndex = value;
                OnPropertyChanged(nameof(HealthIndex));
                OnPropertyChanged(nameof(FillColor));
            }
        }

        public string Message
        {
            get
            {
                if (Math.Abs(HealthIndex - 1.0) < 1e-10)
                {
                    return "Ok";
                }

                if (HealthIndex >= 0.75)
                {
                    return "Degraded";
                }

                return HealthIndex >= 0.5 ? "Badly degraded" : "Error";
            }
        }

        public SolidColorBrush FillColor
        {
            get
            {
                if (Math.Abs(HealthIndex - 1.0) < 1e-10)
                {
                    return Brushes.Green;
                }

                if (HealthIndex >= 0.75)
                {
                    return Brushes.Yellow;
                }

                return HealthIndex >= 0.5 ? Brushes.Orange : Brushes.Red;
            }
        }
        private double _healthIndex;
    }
}