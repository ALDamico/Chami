using System.Windows;

namespace ChamiUI.Windows.AboutBox
{
    public partial class AboutBox
    {
        public AboutBox()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}