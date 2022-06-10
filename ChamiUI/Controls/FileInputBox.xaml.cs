using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;

namespace ChamiUI.Controls
{
    /// <summary>
    /// A control for selecting a file to save.
    /// </summary>
    public partial class FileInputBox
    {
        /// <summary>
        /// Constructs a new <see cref="FileInputBox"/> object.
        /// </summary>
        public FileInputBox()
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(BrowseText))
            {
                BrowseText = DefaultBrowseText;
            }

            FiletextBox.TextChanged += OnTextChanged;
        }

        private const string DefaultBrowseText = "Browse";

        /// <summary>
        /// Handles the TextChanged event in the control.
        /// </summary>
        /// <param name="sender">The object that initiated the event.</param>
        /// <param name="e">Information about the text change.</param>
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            e.Handled = true;
            RoutedEventArgs args = new RoutedEventArgs(FilenameChangedEvent, e);
            RaiseEvent(args);
        }

        /// <summary>
        /// Dependency property for the file name.
        /// </summary>
        public static readonly DependencyProperty FilenameProperty =
            DependencyProperty.Register(nameof(Filename), typeof(string), typeof(FileInputBox));

        /// <summary>
        /// Dependency property for the file filters for the Browse button
        /// </summary>
        public static readonly DependencyProperty FiltersProperty =
            DependencyProperty.Register(nameof(Filters), typeof(string), typeof(FileInputBox));

        public static readonly DependencyProperty BrowseTextProperty =
            DependencyProperty.Register(nameof(BrowseText), typeof(string), typeof(FileInputBox));

        /// <summary>
        /// The full path to the file to save.
        /// </summary>
        public string Filename
        {
            get => (string) GetValue(FilenameProperty);
            set => SetValue(FilenameProperty, value);
        }

        /// <summary>
        /// The file filters for the dialog.
        /// </summary>
        public string Filters
        {
            get => (string) GetValue(FiltersProperty);
            set => SetValue(FiltersProperty, value);
        }

        public string BrowseText
        {
            get => (string) GetValue(BrowseTextProperty);
            set => SetValue(BrowseTextProperty, value);
        }

        /// <summary>
        /// Handles the Click event on the <see cref="BrowseButton"/> element.
        /// </summary>
        /// <param name="sender">The object that initiated the Click event.</param>
        /// <param name="e">Unused.</param>
        private void BrowseButton_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.InitialDirectory = SpecialDirectories.MyDocuments;
            saveFileDialog.AddExtension = true;
            if (!string.IsNullOrEmpty(Filters))
            {
                saveFileDialog.Filter = Filters;
            }

            var shouldSave = saveFileDialog.ShowDialog();
            if (shouldSave.GetValueOrDefault())
            {
                Filename = saveFileDialog.FileName;
            }
        }

        /// <summary>
        /// Event handler for the FilenameChanged event.
        /// </summary>
        public event RoutedEventHandler FilenameChanged
        {
            add { AddHandler(FilenameChangedEvent, value); }
            remove { AddHandler(FilenameChangedEvent, value); }
        }

        /// <summary>
        /// Event that occurs when file <see cref="Filename"/> property changes.
        /// </summary>
        public static readonly RoutedEvent FilenameChangedEvent = EventManager.RegisterRoutedEvent(
            nameof(FilenameChanged), RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(FileInputBox));
    }
}