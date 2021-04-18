using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class ExportWindowViewModel:ViewModelBase
    {
        public ExportWindowViewModel()
        {
            Environments = new ObservableCollection<EnvironmentViewModel>();
        }
        public ExportWindowViewModel(ICollection<EnvironmentViewModel> environments) : this()
        {
            foreach (var environment in environments)
            {
                Environments.Add(environment);
            }
        }
        public ObservableCollection<EnvironmentViewModel> Environments { get; set; }
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
