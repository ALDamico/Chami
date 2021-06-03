using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Controls
{
    public partial class LanguageSelectorControl : UserControl
    {
        public LanguageSelectorControl()
        {
            _viewModel = new LanguageSelectorViewModel();
            DataContext = _viewModel;
            InitializeComponent();
        }

        public LanguageSelectorControl(LanguageSelectorViewModel languageSelectorViewModel)
        {
            _viewModel = languageSelectorViewModel;
            DataContext = _viewModel;
            InitializeComponent();
        }

        private LanguageSelectorViewModel _viewModel;

        private void AvalableLanguagesCombobox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.CurrentLanguage = e.AddedItems[0] as ApplicationLanguageViewModel;
        }
    }
}