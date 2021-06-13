using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using ChamiUI.Annotations;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.PresentationLayer.Filtering
{
    public class EnvironmentNameFilterStrategy : IFilterStrategy, INotifyPropertyChanged
    {
        public EnvironmentNameFilterStrategy()
        {
            Name = ChamiUIStrings.EnvironmentNameFilterStrategyName;
        }

        public string Name { get; set; }

        public string SearchedText
        {
            get => _searchedText;
            set
            {
                _searchedText = value;
                OnPropertyChanged(nameof(SearchedText));
            }
        }

        private string _searchedText;

        public void OnFilter(object sender, FilterEventArgs args)
        {
            args.Accepted = false;
            if (args.Item is EnvironmentViewModel viewModel)
            {
                if (SearchedText == null)
                {
                    args.Accepted = true;
                }
                else if (viewModel.Name.Contains(SearchedText))
                {
                    args.Accepted = true;
                }
            }
        }

        public StringComparison Comparison { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}