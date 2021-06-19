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
            Opposites = new Dictionary<SortDescription, SortDescription>
            {
                [SortByIdAscending] = SortByIdDescending,
                [SortByIdDescending] = SortByIdAscending,
                [SortByNameAscending] = SortByNameDescending,
                [SortByNameDescending] = SortByNameAscending,
                [SortByDateAddedAscending] = SortByDateAddedDescending,
                [SortByDateAddedDescending] = SortByDateAddedAscending
            };
        }

        public static SortDescription GetOppositeSorting(SortDescription sortDescription)
        {
            return Opposites[sortDescription];
        }

        private static readonly Dictionary<SortDescription, SortDescription> Opposites;
        public static SortDescription SortByNameAscending { get; }
        public static SortDescription SortByNameDescending { get; }
        public static SortDescription SortByIdAscending { get; }
        public static SortDescription SortByIdDescending { get; }
        public static SortDescription SortByDateAddedAscending { get; }
        public static SortDescription SortByDateAddedDescending { get; }
    }
}