using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ChamiUI.BusinessLayer.Converters;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Windows.ImportEnvironmentWindow
{
    public partial class ImportEnvironmentWindow
    {
        public ImportEnvironmentWindow(ImportEnvironmentWindowViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = _viewModel;
            InitializeComponent();
        }

        private readonly ImportEnvironmentWindowViewModel _viewModel;

        public void SetEnvironments(IEnumerable<EnvironmentViewModel> viewModels)
        {
            var environmentViewModels = viewModels.ToList();
            if (!environmentViewModels.Any())
            {
                return;
            }

            var converter = new ImportEnvironmentViewModelConverter();
            foreach (var viewModel in environmentViewModels)
            {
                var importViewModel = converter.To(viewModel);
                if (importViewModel != null)
                {
                    importViewModel.ShouldImport = true;
                    _viewModel.NewEnvironments.Add(importViewModel);
                }
               
            }

            _viewModel.SelectedEnvironment ??= _viewModel.NewEnvironments[0];
            _viewModel.UpdatePropertyChanged();
        }

        private void UpdateCheckedStatus(object sender, RoutedEventArgs e)
        {
            _viewModel.UpdatePropertyChanged();
        }
    }
}