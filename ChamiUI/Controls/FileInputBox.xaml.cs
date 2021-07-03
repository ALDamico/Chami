using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;

namespace ChamiUI.Controls
{
    /// <summary>
    /// A control for selecting a file to save.
    /// </summary>
    public partial class FileInputBox : UserControl
    {
        /// <summary>
        /// Constructs a new <see cref="FileInputBox"/> object.
        /// </summary>
        public FileInputBox()
        {
            InitializeComponent();
            FiletextBox.TextChanged += OnTextChanged;
        }

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
        /// Initializes the static properties (i.e., dependency properties) of this control.
        /// </summary>
        static FileInputBox()
        {
            FilenameProperty =
                DependencyProperty.Register(nameof(Filename), typeof(string), typeof(FileInputBox));
            FilenameChangedEvent =
                EventManager.RegisterRoutedEvent(nameof(FilenameChanged), RoutingStrategy.Bubble,
                    typeof(RoutedEventHandler), typeof(FileInputBox));
        }

        /// <summary>
        /// Dependency property for the file name.
        /// </summary>
        public static readonly DependencyProperty FilenameProperty;

        /// <summary>
        /// The full path to the file to save.
        /// </summary>
        public string Filename
        {
            get => (string) GetValue(FilenameProperty);
            set { SetValue(FilenameProperty, value); }
        }

        /// <summary>
        /// Handles the Click event on the <see cref="BrowseButton"/> element.
        /// </summary>
        /// <param name="sender">The object that initiated the Click event.</param>
        /// <param name="e">Unused.</param>
        private void BrowseButton_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.CreatePrompt = true;
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.InitialDirectory = SpecialDirectories.MyDocuments;
            saveFileDialog.AddExtension = true;
            saveFileDialog.Filter = "Excel Open XML File|*.xlsx";
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
        public static readonly RoutedEvent FilenameChangedEvent;
    }
}