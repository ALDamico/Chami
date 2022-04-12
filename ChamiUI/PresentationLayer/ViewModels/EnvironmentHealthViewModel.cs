using System;
using System.Windows.Media;
using ChamiUI.Localization;
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
                OnPropertyChanged(nameof(ToolTip));
                OnPropertyChanged(nameof(HealthIndexPercentage));
            }
        }

        public string HealthIndexPercentage
        {
            get
            {
                var value = HealthIndex * 100;
                return $"{value:F2}%";
            }
        }

        public string ToolTip
        {
            get
            {
                if (Math.Abs(HealthIndex - 1.0) < 1e-10)
                {
                    return ChamiUIStrings.HealthCheckerTooltipOk;
                }

                if (HealthIndex >= 0.75)
                {
                    return ChamiUIStrings.HealthCheckerTooltipDegraded;
                }

                return HealthIndex >= 0.5
                    ? ChamiUIStrings.HealthCheckerTooltipBadlyDegraded
                    : ChamiUIStrings.HealthCheckerTooltipError;
            }
        }

        public string Message
        {
            get
            {
                if (Math.Abs(HealthIndex - 1.0) < 1e-10)
                {
                    return ChamiUIStrings.EnvironmentHealthCheckerMessageOk;
                }

                if (HealthIndex >= 0.75)
                {
                    return ChamiUIStrings.EnvironmentHealthCheckerMessageDegraded;
                }

                return HealthIndex >= 0.5
                    ? ChamiUIStrings.EnvironmentHealthCheckerMessageBadlyDegraded
                    : ChamiUIStrings.EnvironmentHealthCheckerMessageError;
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