using ChamiUI.PresentationLayer.ViewModels;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.Events;

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

        private void AddExampleParagraph(BlockCollection blockCollection, string text, double fontSize, FontFamily fontFamily)
        {
            var paragraph = new Paragraph(new Run(text));
            paragraph.FontFamily = fontFamily;
            paragraph.FontSize = fontSize;
            blockCollection.Add(paragraph);
        }

        /// <summary>
        /// Sets the <see cref="BackgroundColorPicker"/> and <see cref="ForegroundColorPicker"/> colors when
        /// initializing the control.
        /// </summary>
        private void SetColors(ConsoleAppearanceViewModel viewModel)
        {
            if (viewModel == null)
            {
                return;
            }

            if (viewModel.BackgroundColor is SolidColorBrush backgroundColorBrush)
            {
                BackgroundColorPicker.SelectedColor = backgroundColorBrush.Color;
            }

            if (viewModel.ForegroundColor is SolidColorBrush foregroundColorBrush)
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
            var viewModel = GetDataContextAsConsoleAppearanceViewModel();
            if (viewModel == null)
            {
                return;
            }
            viewModel.MinMaxFontSizeChanged += ViewModelOnMinMaxFontSizeChanged;
            SetColors(viewModel);
            PopulateExampleTextBox(viewModel);
        }

        private void ViewModelOnMinMaxFontSizeChanged(object sender, MinMaxFontSizeChangedEventArgs e)
        {
            var viewModel = GetDataContextAsConsoleAppearanceViewModel();
            if (viewModel == null)
            {
                return;
            }
            
            PopulateExampleTextBox(viewModel);
        }

        private void PopulateExampleTextBox(ConsoleAppearanceViewModel viewModel)
        {
            ExampleTextTextBox.Document.Blocks.Clear();

            AddExampleParagraph(ExampleTextTextBox.Document.Blocks, ChamiUIStrings.ExampleTextTextBox_Text, viewModel.FontSize,
                viewModel.FontFamily);
            if (viewModel.MinFontSize != null)
            {
                AddExampleParagraph(ExampleTextTextBox.Document.Blocks, ChamiUIStrings.ExampleTextTextBox_Text,
                    viewModel.MinFontSize.Value, viewModel.FontFamily);
            }

            if (viewModel.MaxFontSize != null)
            {
                AddExampleParagraph(ExampleTextTextBox.Document.Blocks, ChamiUIStrings.ExampleTextTextBox_Text,
                    viewModel.MaxFontSize.Value, viewModel.FontFamily);
            }
        }
    }
}