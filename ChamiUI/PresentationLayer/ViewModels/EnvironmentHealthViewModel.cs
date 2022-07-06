using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using ChamiUI.BusinessLayer.Comparers;
using ChamiUI.BusinessLayer.Converters;
using ChamiUI.BusinessLayer.Validators;
using ChamiUI.Localization;
using Brush = System.Drawing.Brush;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class EnvironmentHealthViewModel : ViewModelBase
    {
        public EnvironmentHealthViewModel()
        {
            HealthStatuses = new ObservableCollection<EnvironmentVariableHealthStatus>();
            WindowColumns = new ObservableCollection<GridViewColumn>();
            ColumnInfoViewModels = new ObservableCollection<ColumnInfoViewModel>();
        }
        public ObservableCollection<EnvironmentVariableHealthStatus> HealthStatuses { get; }
        public ObservableCollection<GridViewColumn> WindowColumns { get; }
        public ObservableCollection<ColumnInfoViewModel> ColumnInfoViewModels { get; }
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

        public void InitWindowColumns(SettingsViewModel appSettings)
        {
            var section = appSettings.HealthCheckSettings;
            WindowColumns.Clear();

            var columns = section.ColumnInfos;
            columns.Sort(new ColumnInfoViewModelComparer());

            var columnConverter = new ColumnInfoConverter();

            foreach (var column in columns)
            {
                if (!column.IsVisible)
                {
                    continue;
                }

                var viewModel = columnConverter.To(column);
                
                GridViewColumn gridViewColumn = new GridViewColumn();
                gridViewColumn.DisplayMemberBinding = viewModel.Binding;
                gridViewColumn.Width = viewModel.ColumnWidth;
                gridViewColumn.Header = viewModel.Header;

                viewModel.GridViewColumn = gridViewColumn;
                ColumnInfoViewModels.Add(viewModel);
                WindowColumns.Add(gridViewColumn);
            }
        }
    }
}