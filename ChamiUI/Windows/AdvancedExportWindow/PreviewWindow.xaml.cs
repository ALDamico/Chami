using System.Windows;
using System.Windows.Input;

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
            DoCopyCommand();
        }

        private void CopyCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CopyCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            DoCopyCommand();
        }

        private void DoCopyCommand()
        {
            Clipboard.SetText(PreviewWindowTextBox.Text);
        }
    }
}