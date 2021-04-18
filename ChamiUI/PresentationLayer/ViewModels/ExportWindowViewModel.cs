using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ChamiUI.BusinessLayer.Converters;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class ExportWindowViewModel : ViewModelBase
    {
        public ExportWindowViewModel()
        {
            ExportAll = true;
            ExportSelected = false;
            Environments = new ObservableCollection<EnvironmentExportWindowViewModel>();
            SelectedEnvironments = new ObservableCollection<EnvironmentExportWindowViewModel>();
        }

        public ExportWindowViewModel(ICollection<EnvironmentViewModel> environments) : this()
        {
            var converter = new EnvironmentExportConverter();
            foreach (var environment in environments)
            {
                var converted = converter.From(environment);
                Environments.Add(converted);
            }
        }

        public ObservableCollection<EnvironmentExportWindowViewModel> Environments { get; set; }

        public bool ExportSelected
        {
            get => _exportSelected;
            set
            {
                _exportSelected = value;
                OnPropertyChanged(nameof(ExportSelected));
            }
        }

        public bool ExportAll
        {
            get => _exportAll;
            set
            {
                _exportAll = value;
                OnPropertyChanged(nameof(ExportAll));
            }
        }

        public void Export()
        {
        }

        public ObservableCollection<EnvironmentExportWindowViewModel> SelectedEnvironments { get; set; }

        public void HandleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var removedItems = e.RemovedItems;
            foreach (var removedItem in removedItems)
            {
                SelectedEnvironments.Remove(removedItem as EnvironmentExportWindowViewModel);
            }
            var addedItems = e.AddedItems;
            foreach (var addedItem in addedItems)
            {
                SelectedEnvironments.Add(addedItem as EnvironmentExportWindowViewModel);
            }
        }

        private bool _exportSelected;
        private bool _exportAll;
    }
}