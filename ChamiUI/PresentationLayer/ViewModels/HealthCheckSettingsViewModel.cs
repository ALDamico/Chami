using System;
using System.Collections.Generic;
using Chami.Db.Entities;
using ChamiUI.BusinessLayer.Annotations;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class HealthCheckSettingsViewModel : SettingCategoryViewModelBase
    {
        public HealthCheckSettingsViewModel()
        {
            ColumnInfos = new List<ColumnInfo>();
        }
        private bool _isEnabled;
        private TimeSpan _timeToCheck;
        
        public List<ColumnInfo> ColumnInfos { get; }


        public TimeSpan TimeToCheck
        {
            get => _timeToCheck;
            set
            {
                _timeToCheck = value;
                OnPropertyChanged(nameof(TimeToCheck));
                OnPropertyChanged(nameof(Milliseconds));
            }
        }

        [NonPersistentSetting]
        public double Milliseconds
        {
            get => TimeToCheck.Milliseconds;
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