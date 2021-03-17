using System.Windows;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using ChamiUI.PresentationLayer;
using ChamiUI.PresentationLayer.Progress;
using Microsoft.Extensions.Configuration;

namespace ChamiUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var dbPath = Directory.GetCurrentDirectory();
            var dbName = "chami.db";
            var connString = $"Data Source={dbPath}/{dbName};Version=3";
            
            ViewModel = new MainWindowViewModel(connString);
            DataContext = ViewModel;
            InitializeComponent();
            
        }
        
        public MainWindowViewModel ViewModel { get; set; }

        private void QuitApplicationMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to quit?", "Exiting Chami.", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.OK)
            {
                Environment.Exit(0);
            }
        }


        private async void ApplyEnvironmentButton_OnClick(object sender, RoutedEventArgs e)
        {
            ConsoleTextBox.Text = "";
            TabControls.SelectedIndex = 1;
            var progress = new Progress<CmdExecutorProgress>((o) =>
            {

                if (o.Message != null)
                {
                    ConsoleTextBox.Text += o.Message;
                    ConsoleTextBox.Text += "\n";
                }

                if (o.OutputStream != null)
                {
                    StreamReader reader = new StreamReader(o.OutputStream);
                    ConsoleTextBox.Text += reader.ReadToEnd();
                    ConsoleTextBox.Text += "\n";
                }
                
            });
            await Task.Run(() => ViewModel.ChangeEnvironmentAsync(progress)) ;
            
        }
    }
}