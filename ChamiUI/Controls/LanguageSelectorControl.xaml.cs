using System.Windows.Controls;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Controls
{
    /// <summary>
    /// The control for changing the application language.
    /// </summary>
    public partial class LanguageSelectorControl
    {
        /// <summary>
        /// Initializes the <see cref="LanguageSelectorControl"/>.
        /// </summary>
        public LanguageSelectorControl()
        {
            _viewModel = new LanguageSelectorViewModel();
            DataContext = _viewModel;
            InitializeComponent();
        }

        /// <summary>
        /// Initializes the <see cref="LanguageSelectorControl"/> and its viewmodel.
        /// </summary>
        /// <param name="languageSelectorViewModel">The starting viewmodel for the control.</param>
        public LanguageSelectorControl(LanguageSelectorViewModel languageSelectorViewModel)
        {
            _viewModel = languageSelectorViewModel;
            DataContext = _viewModel;
            InitializeComponent();
        }

        private LanguageSelectorViewModel _viewModel;

        /// <summary>
        /// Handles the SelectionChanged event in the <see cref="AvalableLanguagesCombobox"/>
        /// </summary>
        /// <param name="sender">The object that initiated the event.</param>
        /// <param name="e">Information about the SelectionChanged event.</param>
        private void AvalableLanguagesCombobox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.CurrentLanguage = e.AddedItems[0] as ApplicationLanguageViewModel;
        }
    }
}