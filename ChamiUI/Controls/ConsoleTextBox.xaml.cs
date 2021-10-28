using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using Chami.CmdExecutor.Progress;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.Factories;
using ChamiUI.PresentationLayer.Utils;

namespace ChamiUI.Controls
{
    public partial class ConsoleTextBox : UserControl
    {
        public ConsoleTextBox()
        {
            InitializeComponent();
        }

        private void ConsoleClearMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            ConsolTextBox.Clear();
        }

        private void ConsoleCopyMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ConsolTextBox.Text))
            {
                return;
            }
            
            var selectedText = ConsolTextBox.SelectedText;
            if (string.IsNullOrWhiteSpace(selectedText))
            {
                selectedText = ConsolTextBox.Text;
            }

            Clipboard.SetText(selectedText);
        }
    
        private void AnimateProgressBar(CmdExecutorProgress o)
        {
            var duration = DurationFactory.FromMilliseconds(250);
            DoubleAnimation doubleAnimation = new DoubleAnimation(o.Percentage, duration);
            ConsoleProgressBar.BeginAnimation(RangeBase.ValueProperty, doubleAnimation);
            ConsoleProgressBar.Value = o.Percentage;
        }

        public void PrintTaskCancelledMessageToConsole()
        {
            SystemSounds.Exclamation.Play();
            ConsolTextBox.Text += ChamiUIStrings.OperationCanceledMessage;
            ConsolTextBox.Text += "Reverting back to previous environment.";
            ConsoleProgressBar.Foreground = System.Windows.Media.Brushes.Red;
        }

        public void ClearConsole()
        {
            ConsolTextBox.Text = "";
        }

        public void HandleProgressReport(CmdExecutorProgress o)
        {
            
            if (o.Message != null)
            {
                var message = o.Message;
                message.TrimStart('\n');
                if (!o.Message.EndsWith("\n"))
                {
                    message += "\n";
                }

                ConsolTextBox.Text += message;
            }

            if (o.OutputStream != null)
            {
                StreamReader reader = new StreamReader(o.OutputStream);
                ConsolTextBox.Text += reader.ReadToEnd();
            }

            ConsolTextBox.ScrollToEnd();
            AnimateProgressBar(o);
        }
        
        public void ResetProgressBar()
        {
            //Avoids animating the progressbar when its value is reset to zero.
            ConsoleProgressBar.BeginAnimation(RangeBase.ValueProperty, null);
            ConsoleProgressBar.Value = 0.0;

            ConsoleProgressBar.Foreground = ResourceUtils.DefaultProgressBarColor;
        }
    }
}