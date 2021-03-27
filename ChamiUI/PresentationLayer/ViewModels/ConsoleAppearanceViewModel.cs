using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using System.Text.RegularExpressions;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public class ConsoleAppearanceViewModel: ViewModelBase
    {
        public ConsoleAppearanceViewModel()
        {
            var installedFonts = System.Windows.Media.Fonts.GetFontFamilies("c:/windows/fonts");
            FontFamily = installedFonts.FirstOrDefault(f => Regex.Match(f.Source, "segoe").Success);
            FontSize = 12.0;
            BackgroundColor = Brushes.Black;
            ForegroundColor = Brushes.White;
            FontFamilies = new ObservableCollection<FontFamily>(Fonts.GetFontFamilies("c:/windows/fonts"));
        }
        
        public ObservableCollection<FontFamily> FontFamilies { get; }
        
        private FontFamily _fontFamily;
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
        public Brush ForegroundColor
        {
            get => _foregroundColor;
            set
            {
                _foregroundColor = value;
                OnPropertyChanged(nameof(ForegroundColor));
            }
        }

        public void ChangeBackgroundColor(SolidColorBrush brush)
        {
            BackgroundColor = brush;
        }
    }
}
