using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;
using ChamiUI.PresentationLayer.Utils;

namespace ChamiUI.PresentationLayer.ViewModels
{
    /// <summary>
    /// Contains information about how the console should appear in the application.
    /// </summary>
    public class ConsoleAppearanceViewModel : GenericLabelViewModel
    {
        /// <summary>
        /// Constructs a new <see cref="ConsoleAppearanceViewModel"/> object, retrieves the list of installed fonts on
        /// the machine and sets its default values.
        /// </summary>
        public ConsoleAppearanceViewModel()
        {
            FontFamilies = SettingsUtils.GetInstalledFonts();
            FontFamily = FontFamilies.FirstOrDefault(f => Regex.Match(f.Source, "segoe").Success);
            FontSize = 12.0;
            BackgroundColor = Brushes.Black;
            ForegroundColor = Brushes.White;
        }

        

        /// <summary>
        /// A list of all font families installed on this machine.
        /// </summary>
        public ObservableCollection<FontFamily> FontFamilies { get; }

        private FontFamily _fontFamily;
        
        /// <summary>
        /// The font that the console will use.
        /// </summary>
        public FontFamily FontFamily
        {
            get => _fontFamily;
            set
            {
                _fontFamily = value;
                OnPropertyChanged(nameof(FontFamily));
            }
        }

        private double _fontSize;
        
        /// <summary>
        /// The font size the console will use.
        /// </summary>
        public double FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = value;
                OnPropertyChanged(nameof(FontSize));
            }
        }

        private Brush _backgroundColor;
        
        /// <summary>
        /// The background color of the console.
        /// </summary>
        public Brush BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;
                OnPropertyChanged(nameof(BackgroundColor));
            }
        }

        private Brush _foregroundColor;
        
        /// <summary>
        /// The foreground (i.e., text) color of the console.
        /// </summary>
        public Brush ForegroundColor
        {
            get => _foregroundColor;
            set
            {
                _foregroundColor = value;
                OnPropertyChanged(nameof(ForegroundColor));
            }
        }

        /// <summary>
        /// Sets the <see cref="BackgroundColor"/> property to a new value.
        /// </summary>
        /// <param name="brush">The new color.</param>
        public void ChangeBackgroundColor(SolidColorBrush brush)
        {
            BackgroundColor = brush;
        }

        /// <summary>
        /// Sets the <see cref="ForegroundColor"/> property to a new value.
        /// </summary>
        /// <param name="brush">The new color.</param>
        public void ChangeForegroundColor(SolidColorBrush brush)
        {
            ForegroundColor = brush;
        }
    }
}
