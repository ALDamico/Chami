using ChamiUI.PresentationLayer.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using ChamiUI.Localization;

namespace ChamiUI.Controls
{
    /// <summary>
    /// This is the control that allows the user to customize the console in the main window.
    /// </summary>
    public partial class ConsoleAppearanceEditor : UserControl
    {
        /// <summary>
        /// Constructs a new <see cref="ConsoleAppearanceEditor"/> control and sets its viewmodel.
        /// </summary>
        /// <param name="viewModel">The object containing the control's state.</param>
        public ConsoleAppearanceEditor(ConsoleAppearanceViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = viewModel;
            
            InitializeComponent();
            SetColors();
        }

        /// <summary>
        /// Sets the <see cref="BackgroundColorPicker"/> and <see cref="ForegroundColorPicker"/> colors when
        /// initializing the control.
        /// </summary>
        private void SetColors()
        {
            if (_viewModel.BackgroundColor is SolidColorBrush backgroundColorBrush)
            {
                BackgroundColorPicker.SelectedColor = backgroundColorBrush.Color;
            }

            if (_viewModel.ForegroundColor is SolidColorBrush foregroundColorBrush)
            {
                ForegroundColorPicker.SelectedColor = foregroundColorBrush.Color;
            }
        }

        private readonly ConsoleAppearanceViewModel _viewModel;

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
                _viewModel.ChangeBackgroundColor(brush);
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
                _viewModel.ChangeForegroundColor(brush);
            }
        }
    }
}