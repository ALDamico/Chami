using System.Windows;

namespace ChamiUI.Windows.EnvironmentHealth
{
    public partial class EnvironmentHealthWindow : Window
    {
        public EnvironmentHealthWindow()
        {
            InitializeComponent();
        }

        private void EnvironmentHealthWindowCloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}