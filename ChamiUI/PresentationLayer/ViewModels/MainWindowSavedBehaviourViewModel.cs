﻿using System.ComponentModel;
using ChamiUI.BusinessLayer.Annotations;
using ChamiUI.PresentationLayer.Filtering;
using ChamiUI.PresentationLayer.Minimizing;

namespace ChamiUI.PresentationLayer.ViewModels
{
    [ExplicitSaveOnly]
    public class MainWindowSavedBehaviourViewModel:SettingCategoryViewModelBase
    {
        private bool _isCaseSensitiveSearch;

        public bool IsCaseSensitiveSearch
        {
            get => _isCaseSensitiveSearch;
            set
            {
                _isCaseSensitiveSearch = value;
                OnPropertyChanged(nameof(IsCaseSensitiveSearch));
            }
        }
        
        public double Height { get; set; }
        public double Width { get; set; }
        public double XPosition { get; set; }
        public double YPosition { get; set; }
        public IFilterStrategy SearchPath { get; set; }
        public SortDescription SortDescription { get; set; }
        public IMinimizationStrategy MinimizationStrategy { get; set; }
    }
}