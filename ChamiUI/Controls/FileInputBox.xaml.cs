using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;

namespace ChamiUI.Controls
{
    public partial class FileInputBox : UserControl
    {
        public FileInputBox()
        {
            InitializeComponent();
            FiletextBox.TextChanged += OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            e.Handled = true;
            RoutedEventArgs args = new RoutedEventArgs(FilenameChangedEvent, e);
            RaiseEvent(args);
        }


        static FileInputBox()
        {
            FilenameProperty =
                DependencyProperty.Register(nameof(Filename), typeof(string), typeof(FileInputBox));
        }

        public static readonly DependencyProperty FilenameProperty;

        public string Filename
        {
            get => (string) GetValue(FilenameProperty);
            set { SetValue(FilenameProperty, value); }
        }

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
        
        public event RoutedEventHandler FilenameChanged
        {
            add {AddHandler(FilenameChangedEvent, value);}
            remove {AddHandler(FilenameChangedEvent, value);}
        }

        public static readonly RoutedEvent FilenameChangedEvent =
            EventManager.RegisterRoutedEvent(nameof(FilenameChanged), RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(FileInputBox));
    }
}