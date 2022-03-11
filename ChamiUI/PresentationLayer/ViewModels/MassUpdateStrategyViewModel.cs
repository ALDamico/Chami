using System;
using System.Threading.Tasks;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.Localization;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class MassUpdateStrategyViewModel : ViewModelBase
    {
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private static MassUpdateStrategyViewModel _defaultUpdateStrategy;

        public static MassUpdateStrategyViewModel DefaultUpdateStrategy
        {
            get
            {
                if (_defaultUpdateStrategy == null)
                {
                    _defaultUpdateStrategy = new MassUpdateStrategyViewModel()
                        { Name = ChamiUIStrings.MassUpdateStrategyName_UpdateAll };
                }

                return _defaultUpdateStrategy;
            }
        }
    }
}