using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChamiUI.BusinessLayer.Converters;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class ExportWindowViewModel:ViewModelBase
    {
        public ExportWindowViewModel()
        {
            ExportAll = true;
            ExportSelected = false;
            Environments = new ObservableCollection<EnvironmentExportWindowViewModel>();
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
        public bool ExportSelected { get => _exportSelected;
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

        private bool _exportSelected;
        private bool _exportAll;
    }
}
