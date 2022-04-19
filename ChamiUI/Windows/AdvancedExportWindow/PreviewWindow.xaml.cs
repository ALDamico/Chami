using System.Windows;

namespace ChamiUI.Windows.AdvancedExportWindow
{
    public partial class PreviewWindow : Window
    {
        public PreviewWindow()
        {
            InitializeComponent();
        }

        private void PreviewWindowCloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PreviewWindowCopyButton_OnClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(PreviewWindowTextBox.Text);
        }
    }
}