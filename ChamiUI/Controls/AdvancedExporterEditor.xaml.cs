using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.Controls
{
    public partial class AdvancedExporterEditor : UserControl
    {
        public AdvancedExporterEditor()
        {
            InitializeComponent();
            SetColors();
        }

        private void AdvancedExporterEditorFgColorPicker_OnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            var dataContext = GetDataContext();

            var newColor = e.NewValue;
            if (newColor.HasValue)
            {
                dataContext.ChangeForegroundColor(newColor.Value);                
            }
        }

        private AdvancedExporterSettingsViewModel GetDataContext()
        {
            return DataContext as AdvancedExporterSettingsViewModel;
        }

        private void AdvancedExporterEditorBgColorPicker_OnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            var dataContext = GetDataContext();

            var newColor = e.NewValue;
            if (newColor.HasValue)
            {
                dataContext.ChangeBackgroundColor(newColor.Value);                
            }
        }
        
        private void SetColors()
        {

            var dataContext = GetDataContext();
            if (dataContext == null)
            {
                return;
            }

            if (dataContext.PreviewBackgroundColor is SolidColorBrush backgroundColorBrush)
            {
                AdvancedExporterEditorBgColorPicker.SelectedColor = backgroundColorBrush.Color;
            }

            if (dataContext.PreviewForegroundColor is SolidColorBrush foregroundColorBrush)
            {
                AdvancedExporterEditorFgColorPicker.SelectedColor = foregroundColorBrush.Color;
            }
        }

        private void AdvancedExporterEditor_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SetColors();
        }
    }
}