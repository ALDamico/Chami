using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ChamiUI.PresentationLayer;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.Progress;

namespace ChamiUI.Windows.MainWindow
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

        private void NewEnvironmentMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var childWindow = new NewEnvironmentWindow.NewEnvironmentWindow();
            childWindow.EnvironmentSaved += OnEnvironmentSaved;
            childWindow.ShowDialog();
        }

        private void OnEnvironmentSaved(object sender, EnvironmentSavedEventArgs args)
        {
            if (args != null)
            {
                ViewModel.Environments.Add(args.EnvironmentViewModel);
            }
        }

        private void EditEnvironmentMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.EnableEditing();
        }

        private void EnvironmentsListbox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.DisableEditing();
        }

        private void DeleteEnvironmentMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedEnvironmentName = ViewModel.SelectedEnvironment.Name;
            var selectedEnvironmentVariableCount = ViewModel.SelectedEnvironment.EnvironmentVariables.Count;
            string message;
            if (selectedEnvironmentVariableCount == 0)
            {
                message = $"Are you sure you want to remove the environment {selectedEnvironmentName}?";
            }
            else
            {
                message =
                    $"Are you sure you want to remove the environment {selectedEnvironmentName} and its {selectedEnvironmentVariableCount} variables?";
            }

            var result = MessageBox.Show( message,"Confirm deletion", MessageBoxButton.OKCancel,
                MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                ViewModel.DeleteSelectedEnvironment();
            }
        }
    }
}