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
            InitializeComponent();
        }

        /// <summary>
        /// Initializes the <see cref="LanguageSelectorControl"/> and its viewmodel.
        /// </summary>
        /// <param name="languageSelectorViewModel">The starting viewmodel for the control.</param>
        public LanguageSelectorControl(LanguageSelectorViewModel languageSelectorViewModel)
        {
            DataContext = languageSelectorViewModel;
            InitializeComponent();
        }
    }
}