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
            var backgroundColorBrush = (_viewModel.BackgroundColor as SolidColorBrush);
            if (backgroundColorBrush != null)
            {
                BackgroundColorPicker.SelectedColor = backgroundColorBrush.Color;
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
    }
}