using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;
using ChamiUI.PresentationLayer.Events;

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
            FontFamilies = GetInstalledFonts();
            FontFamily = FontFamilies.FirstOrDefault(f => Regex.Match(f.Source, "segoe").Success);
            FontSize = 12.0;
            BackgroundColor = Brushes.Black;
            ForegroundColor = Brushes.White;
        }

        /// <summary>
        /// Retrieves all font families installed on this machine.
        /// </summary>
        /// <returns>An <see cref="ObservableCollection{T}"/> containing all installed fonts in the current machine.</returns>
        private ObservableCollection<FontFamily> GetInstalledFonts()
        {
            return new ObservableCollection<FontFamily>(Fonts.GetFontFamilies("c:/windows/fonts"));
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

        private double? _minFontSize;

        public double? MinFontSize
        {
            get => _minFontSize;
            set
            {
                _minFontSize = value;
                OnPropertyChanged();
                MinMaxFontSizeChanged?.Invoke(this, new MinMaxFontSizeChangedEventArgs(MinFontSize, MaxFontSize));
            }
        }

        private double? _maxFontSize;

        public double? MaxFontSize
        {
            get => _maxFontSize;
            set
            {
                _maxFontSize = value;
                OnPropertyChanged();
                MinMaxFontSizeChanged?.Invoke(this, new MinMaxFontSizeChangedEventArgs(MinFontSize, MaxFontSize));
            }
        }

        public const double FontSizeChangeStep = 1.0;

        private bool _saveFontSizeOnApplicationExit;

        public bool SaveFontSizeOnApplicationExit
        {
            get => _saveFontSizeOnApplicationExit;
            set
            {
                _saveFontSizeOnApplicationExit = value;
                OnPropertyChanged();
            }
        }

        private bool _enableFontSizeResizingWithScrollWheel;

        public bool EnableFontSizeResizingWithScrollWheel
        {
            get => _enableFontSizeResizingWithScrollWheel;
            set
            {
                _enableFontSizeResizingWithScrollWheel = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsSaveFontSizeCheckboxEnabled));
                OnPropertyChanged(nameof(IsMinFontSizeBoxEnabled));
                OnPropertyChanged(nameof(IsMaxFontSizeBoxEnabled));
            }
        }

        public bool IsSaveFontSizeCheckboxEnabled => EnableFontSizeResizingWithScrollWheel;
        public bool IsMinFontSizeBoxEnabled => EnableFontSizeResizingWithScrollWheel;
        public bool IsMaxFontSizeBoxEnabled => EnableFontSizeResizingWithScrollWheel;

        public event EventHandler<MinMaxFontSizeChangedEventArgs> MinMaxFontSizeChanged;
    }
}
