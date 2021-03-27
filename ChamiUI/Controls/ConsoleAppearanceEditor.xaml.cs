using System.Windows;
using ChamiUI.PresentationLayer.ViewModels;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChamiUI.Controls
{
    public partial class ConsoleAppearanceEditor : UserControl
    {
        public ConsoleAppearanceEditor(ConsoleAppearanceViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = viewModel;

            InitializeComponent();
            SetColors();
        }

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

        private ConsoleAppearanceViewModel _viewModel;

        private void ColorPicker_OnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            var newColor = e.NewValue;
            if (newColor != null)
            {
                var brush = new SolidColorBrush(newColor.Value);
                _viewModel.ChangeBackgroundColor(brush);
            }
        }

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