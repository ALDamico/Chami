using System;
using System.Collections.Generic;
using System.ComponentModel;
using ChamiUI.PresentationLayer.ViewModels;


namespace ChamiUI.PresentationLayer.Utils
{
    public static class SortDescriptionUtils
    {
        static SortDescriptionUtils()
        {
            SortByNameAscending = new SortDescription(nameof(EnvironmentViewModel.Name), ListSortDirection.Ascending);
            SortByNameDescending = new SortDescription(nameof(EnvironmentViewModel.Name), ListSortDirection.Descending);
            SortByIdAscending = new SortDescription(nameof(EnvironmentViewModel.Id), ListSortDirection.Ascending);
            SortByIdDescending = new SortDescription(nameof(EnvironmentViewModel.Id), ListSortDirection.Descending);
            SortByDateAddedAscending =
                new SortDescription(nameof(EnvironmentViewModel.AddedOn), ListSortDirection.Ascending);
            SortByDateAddedDescending =
                new SortDescription(nameof(EnvironmentViewModel.AddedOn), ListSortDirection.Descending);
            _opposites = new Dictionary<SortDescription, SortDescription>();
            _opposites[SortByIdAscending] = SortByIdDescending;
            _opposites[SortByIdDescending] = SortByIdAscending;
            _opposites[SortByNameAscending] = SortByNameDescending;
            _opposites[SortByNameDescending] = SortByNameAscending;
            _opposites[SortByDateAddedAscending] = SortByDateAddedDescending;
            _opposites[SortByDateAddedDescending] = SortByDateAddedAscending;
        }

        public static SortDescription GetOppositeSorting(SortDescription sortDescription)
        {
            return _opposites[sortDescription];
        }

        private static Dictionary<SortDescription, SortDescription> _opposites;
        public static SortDescription SortByNameAscending { get; }
        public static SortDescription SortByNameDescending { get; }
        public static SortDescription SortByIdAscending { get; }
        public static SortDescription SortByIdDescending { get; }
        public static SortDescription SortByDateAddedAscending { get; }
        public static SortDescription SortByDateAddedDescending { get; }
    }
}