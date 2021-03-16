using System.Windows;
using System;
using System.IO;
using System.Windows.Controls;
using ChamiUI.PresentationLayer;
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


        private void ApplyEnvironmentButton_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.ChangeEnvironment();
        }
    }
}