using ChamiUI.BusinessLayer.Factories;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.Progress;
using ChamiUI.PresentationLayer.ViewModels;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ChamiUI.BusinessLayer;

namespace ChamiUI.Windows.MainWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var connectionString = App.GetConnectionString();

            ViewModel = new MainWindowViewModel(connectionString);
            ViewModel.EnvironmentExists += OnEnvironmentExists;
            DataContext = ViewModel;
            InitializeComponent();
        }

        private void OnEnvironmentExists(object sender, EnvironmentExistingEventArgs e)
        {
            if (e.Exists)
            {
                MessageBox.Show("The environment you're trying to import already exists!",
                    "Error importing environment", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public MainWindowViewModel ViewModel { get; set; }

        private void QuitApplicationMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to quit?", "Exiting Chami.", MessageBoxButton.OKCancel,
                MessageBoxImage.Question);
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
                }

                if (o.OutputStream != null)
                {
                    StreamReader reader = new StreamReader(o.OutputStream);
                    ConsoleTextBox.Text += reader.ReadToEnd();
                }

                ConsoleTextBox.ScrollToEnd();
            });
            await Task.Run(() => ViewModel.ChangeEnvironmentAsync(progress));
            var watchedApplicationSettings = ViewModel.Settings.WatchedApplicationSettings;
            if (watchedApplicationSettings.IsDetectionEnabled)
            {
                var message = ViewModel.GetDetectedApplicationsMessage();
                MessageBox.Show(message, "Restart applications!", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
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

            var result = MessageBox.Show(message, "Confirm deletion", MessageBoxButton.OKCancel,
                MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                ViewModel.DeleteSelectedEnvironment();
            }
        }

        private void SaveCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.SaveCurrentEnvironment();
        }

        private void SaveCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ViewModel.SelectedVariable?.Name == null || ViewModel.SelectedVariable.Value == null)
            {
                e.CanExecute = false;
                return;
            } 
            if (ViewModel.SelectedEnvironment != null && ViewModel.EditingEnabled)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void ImportFromJsonMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = OpenFileDialogFactory.GetOpenFileDialog("Json files|*.json");
            var fileSelected = openFileDialog.ShowDialog();

            if (fileSelected != null && fileSelected.Value)
            {
                var file = openFileDialog.OpenFile();
                try
                {
                    ViewModel.ImportJson(file);
                }
                catch (JsonSerializationException ex)
                {
                    MessageBox.Show("Unable to deserialize input file!\nSee the log for more details.",
                        "Deserialization error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    var logger = ((App.Current) as ChamiUI.App).GetLogger();
                    logger.Error(ex.Message);
                    logger.Error(ex.StackTrace);
                }
            }
        }

        private void SettingsMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var childWindow = new SettingsWindow.SettingsWindow();
            childWindow.SettingsSaved += OnSettingsSaved;
            childWindow.ShowDialog();
        }

        private void OnSettingsSaved(object sender, SettingsSavedEventArgs args)
        {
            ViewModel.Settings = args.Settings;
        }

        private void BackupEnvironmentMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.BackupEnvironment();
        }

        private void ImportFromDotenvMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = OpenFileDialogFactory.GetOpenFileDialog("DotEnv files|*.env");
            var fileSelected = openFileDialog.ShowDialog();

            if (fileSelected != null && fileSelected.Value)
            {
                ViewModel.ImportDotEnv(openFileDialog.FileName);
            }
        }

        private void AboutMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            new AboutBox.AboutBox().ShowDialog();
        }

        private void WebsiteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("www.lucianodamico.info");
            // Required, otherwise the app crashes with a Win32Exception
            startInfo.UseShellExecute = true;
            Process process = new Process();
            process.StartInfo = startInfo;

            process.Start();
        }

        private void CopyEnvironmentVariableMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(ViewModel.SelectedVariable.Value);
        }

        private void DeleteEnvironmentVariableMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.DeleteSelectedVariable();
        }
    }
}