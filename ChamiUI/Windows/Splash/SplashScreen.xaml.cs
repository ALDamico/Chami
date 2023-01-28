using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using ChamiUI.PresentationLayer.Progress;

namespace ChamiUI.Windows.Splash;

public partial class SplashScreen : Window
{
    public SplashScreen()
    {
        //Background = new SolidColorBrush(new Color() {A = 0, B = 255, G = 255, R = 255});
        InitializeComponent();
    }

    public void OnMessageReceived(AppLoadProgress progress)
    {
        Dispatcher.InvokeAsync(() =>
        {
            ProgressBar.Value = progress.Percentage;
            MessageLabel.Content = progress.Message;
            //UpdateLayout();
        });
    }
}