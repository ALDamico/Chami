using System.Windows;

namespace ChamiUI.Windows.AboutBox
{
    public partial class AboutBox
    {
        public AboutBox(Window owner)
        {
            Owner = owner;
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}