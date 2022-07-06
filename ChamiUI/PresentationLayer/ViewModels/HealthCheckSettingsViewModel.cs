using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Chami.Db.Entities;
using ChamiUI.BusinessLayer.Annotations;
using ChamiUI.BusinessLayer.Converters;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class HealthCheckSettingsViewModel : SettingCategoryViewModelBase
    {
        public HealthCheckSettingsViewModel()
        {
            ColumnInfos = new List<ColumnInfo>();
            ColumnInfoViewModels = new ObservableCollection<ColumnInfoViewModel>();
            SelectedColumnInfoViewModels = new List<ColumnInfoViewModel>();
        }
        private bool _isEnabled;
        private TimeSpan _timeToCheck;
        private ColumnInfoViewModel _selectedColumnInfoViewModel;

        public ColumnInfoViewModel SelectedColumnInfoViewModel
        {
            get => _selectedColumnInfoViewModel;
            set
            {
                _selectedColumnInfoViewModel = value;
                OnPropertyChanged(nameof(SelectedColumnInfoViewModel));
                OnPropertyChanged(nameof(ToggleVisibilityButtonEnabled));
                OnPropertyChanged(nameof(MoveColumnUpButtonIsEnabled));
                OnPropertyChanged(nameof(MoveColumnDownButtonIsEnabled));
            }
        }
        
        public List<ColumnInfo> ColumnInfos { get; }
        public ObservableCollection<ColumnInfoViewModel> ColumnInfoViewModels { get; }


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

        public bool ToggleVisibilityButtonEnabled => SelectedColumnInfoViewModel != null;

        public bool MoveColumnUpButtonIsEnabled => SelectedColumnInfoViewModel != null && SelectedColumnInfoViewModels.Count <= 1;

        public bool MoveColumnDownButtonIsEnabled => SelectedColumnInfoViewModel != null  && SelectedColumnInfoViewModels.Count <= 1;

        public List<ColumnInfoViewModel> SelectedColumnInfoViewModels { get; }

        public void InitColumnInfoViewModels()
        {
            ColumnInfoViewModels.Clear();

            var converter = new ColumnInfoConverter();
            foreach (var column in ColumnInfos)
            {
                var viewModel = converter.To(column);

                ColumnInfoViewModels.Add(viewModel);
            }
        }

        private void MoveSelectedColumnUpInner(ColumnInfoViewModel columnInfoViewModel)
        {
            var currentOrdinalPosition = columnInfoViewModel.OrdinalPosition;

            if (currentOrdinalPosition == 0)
            {
                return;
            }

            ColumnInfoViewModel otherColumnInfo = null;

            foreach (ColumnInfoViewModel viewModel in ColumnInfoViewModels.Where(ci =>
                         ci.OrdinalPosition < currentOrdinalPosition))
            {
                if (viewModel.OrdinalPosition < currentOrdinalPosition && (otherColumnInfo == null ||
                                                                           otherColumnInfo.OrdinalPosition <
                                                                           viewModel.OrdinalPosition))
                {
                    otherColumnInfo = viewModel;
                }
            }

            if (otherColumnInfo != null)
            {
                SelectedColumnInfoViewModel.OrdinalPosition = otherColumnInfo.OrdinalPosition;
                otherColumnInfo.OrdinalPosition = currentOrdinalPosition;
            }
        }

        public void MoveSelectedColumnUp()
        {
            List<ColumnInfoViewModel> columnsToProcess = new List<ColumnInfoViewModel>();
            if (SelectedColumnInfoViewModels.Count <= 1)
            {
                columnsToProcess.Add(SelectedColumnInfoViewModel);
            }
            else
            {
                foreach (var column in SelectedColumnInfoViewModels)
                {
                    columnsToProcess.Add(column);
                }
            }

            foreach (var column in columnsToProcess)
            {
                MoveSelectedColumnUpInner(column);
            }
        }

        public void ToggleCurrentColumnVisibility()
        {
            if (SelectedColumnInfoViewModels.Count > 0)
            {
                foreach (var viewModel in SelectedColumnInfoViewModels)
                {
                    viewModel.IsVisible = !viewModel.IsVisible;
                }

                return;
            }
            if (SelectedColumnInfoViewModel == null)
            {
                return;
            }

            SelectedColumnInfoViewModel.IsVisible = !SelectedColumnInfoViewModel.IsVisible;
        }

        private void MoveSelectedColumnDownInner(ColumnInfoViewModel targetColumnInfoViewModel)
        {
            var currentOrdinalPosition = targetColumnInfoViewModel.OrdinalPosition;
            var minOrdinalPosition = ColumnInfoViewModels.Max(ci => ci.OrdinalPosition);

            if (currentOrdinalPosition == ColumnInfoViewModels.Count)
            {
                return;
            }

            ColumnInfoViewModel otherColumnInfo = null;

            foreach (ColumnInfoViewModel viewModel in ColumnInfoViewModels.Where(ci =>
                         ci.OrdinalPosition > minOrdinalPosition))
            {
                if (viewModel.OrdinalPosition > currentOrdinalPosition && (otherColumnInfo == null ||
                                                                           otherColumnInfo.OrdinalPosition >
                                                                           viewModel.OrdinalPosition))
                {
                    otherColumnInfo = viewModel;
                }
            }

            if (otherColumnInfo != null)
            {
                targetColumnInfoViewModel.OrdinalPosition = otherColumnInfo.OrdinalPosition;
                otherColumnInfo.OrdinalPosition = currentOrdinalPosition;
            }
        }

        public void MoveSelectedColumnDown()
        {
            List<ColumnInfoViewModel> columnsToProcess = new List<ColumnInfoViewModel>();
            if (SelectedColumnInfoViewModels.Count <= 1)
            {
                columnsToProcess.Add(SelectedColumnInfoViewModel);
            }
            else
            {
                foreach (var column in SelectedColumnInfoViewModels)
                {
                    columnsToProcess.Add(column);
                }
            }

            foreach (var column in columnsToProcess)
            {
                MoveSelectedColumnDownInner(column);
            }
        }

        public void UpdateSelection(IList addedItems, IList removedItems)
        {
            foreach (ColumnInfoViewModel removedItem in removedItems)
            {
                SelectedColumnInfoViewModels.Remove(removedItem);
            }

            foreach (ColumnInfoViewModel addedItem in addedItems)
            {
                SelectedColumnInfoViewModels.Add(addedItem);
            }
            
            OnPropertyChanged(nameof(SelectedColumnInfoViewModels));
            // Required because the propertyChanged event for SelectedColumnInfoViewModel fires before this method call
            OnPropertyChanged(nameof(SelectedColumnInfoViewModel));
        }
    }
}