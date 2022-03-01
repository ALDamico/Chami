using ChamiUI.PresentationLayer.ViewModels;
using System.Windows;
using System.Windows.Media;

namespace ChamiUI.Controls
{
    /// <summary>
    /// This is the control that allows the user to customize the console in the main window.
    /// </summary>
    public partial class ConsoleAppearanceEditor
    {
        /// <summary>
        /// Constructs a new <see cref="ConsoleAppearanceEditor"/> control.
        /// </summary>
        public ConsoleAppearanceEditor()
        {
            InitializeComponent();
            //SetColors();
        }

        /// <summary>
        /// Sets the <see cref="BackgroundColorPicker"/> and <see cref="ForegroundColorPicker"/> colors when
        /// initializing the control.
        /// </summary>
        private void SetColors()
        {
            if (GetDataContextAsConsoleAppearanceViewModel() == null)
            {
                return;
            }

            if (GetDataContextAsConsoleAppearanceViewModel().BackgroundColor is SolidColorBrush backgroundColorBrush)
            {
                BackgroundColorPicker.SelectedColor = backgroundColorBrush.Color;
            }

            if (GetDataContextAsConsoleAppearanceViewModel().ForegroundColor is SolidColorBrush foregroundColorBrush)
            {
                ForegroundColorPicker.SelectedColor = foregroundColorBrush.Color;
            }
        }

        /// <summary>
        /// Handles color changing on the <see cref="BackgroundColorPicker"/>.
        /// </summary>
        /// <param name="sender">The object that started this action.</param>
        /// <param name="e">Information about the new color.</param>
        private void ColorPicker_OnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            var newColor = e.NewValue;
            if (newColor != null)
            {
                var brush = new SolidColorBrush(newColor.Value);
                GetDataContextAsConsoleAppearanceViewModel().ChangeBackgroundColor(brush);
            }
        }

        /// <summary>
        /// Handles color changing on the <see cref="ForegroundColorPicker"/>.
        /// </summary>
        /// <param name="sender">The object that started this action.</param>
        /// <param name="e">Information about the new color.</param>
        private void ForegroundColorPicker_OnSelectedColorChanged(object sender,
            RoutedPropertyChangedEventArgs<Color?> e)
        {
            var newColor = e.NewValue;
            if (newColor != null)
            {
                var brush = new SolidColorBrush(newColor.Value);
                GetDataContextAsConsoleAppearanceViewModel().ChangeForegroundColor(brush);
            }
        }

        public ConsoleAppearanceViewModel GetDataContextAsConsoleAppearanceViewModel()
        {
            return DataContext as ConsoleAppearanceViewModel;
        }

        private void ConsoleAppearanceEditor_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SetColors();
        }
    }
}