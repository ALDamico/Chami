using ChamiUI.BusinessLayer.Factories;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.Progress;
using ChamiUI.PresentationLayer.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            FocusConsoleTab();
            var progress = new Progress<CmdExecutorProgress>(HandleProgressReport);
            await Task.Run(() => ViewModel.ChangeEnvironmentAsync(progress));
            var watchedApplicationSettings = ViewModel.Settings.WatchedApplicationSettings;
            if (watchedApplicationSettings.IsDetectionEnabled)
            {
                var message = ViewModel.GetDetectedApplicationsMessage();
                if (message != null)
                {
                    MessageBox.Show(message, "Restart applications!", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
        }

        private void FocusConsoleTab(bool clearTextBox = true)
        {
            if (clearTextBox)
            {
                ConsoleTextBox.Text = "";
            }
            
            TabControls.SelectedIndex = CONSOLE_TAB_INDEX;
        }

        private void HandleProgressReport(CmdExecutorProgress o)
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
            FocusEnvironmentVariablesTab();
        }

        private const int ENVIRONMENT_VARIABLES_TAB_INDEX = 0;
        private const int CONSOLE_TAB_INDEX = 1;

        private void FocusEnvironmentVariablesTab()
        {
            TabControls.SelectedIndex = ENVIRONMENT_VARIABLES_TAB_INDEX;
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

        private void NewEnvironmentCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ViewModel.EditingEnabled)
            {
                e.CanExecute = false;
            }
            else
            {
                e.CanExecute = true;
            }
        }

        private void SaveCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
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
            var selectedEnvironmentVariables = new List<object>();
            foreach (var envVar in CurrentEnvironmentVariablesDataGrid.SelectedItems)
            {
                selectedEnvironmentVariables.Add(envVar);
            }
            
            foreach (var environmentVariable in selectedEnvironmentVariables)
            {
                if (environmentVariable is EnvironmentVariableViewModel vm)
                {
                    ViewModel.SelectedVariable = vm;
                    ViewModel.DeleteSelectedVariable();
                }
            }
            
        }

        private void NewEnvironmentCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var childWindow = new NewEnvironmentWindow.NewEnvironmentWindow();
            childWindow.EnvironmentSaved += OnEnvironmentSaved;
            childWindow.ShowDialog();
        }

        private async void ResetVarsMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var response =
                MessageBox.Show(
                    "This will remove the currently active Chami environment. Are you sure you want to proceed?",
                    "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Hand, MessageBoxResult.No);
            if (response == MessageBoxResult.Yes)
            {
                var progress = new Progress<CmdExecutorProgress>(HandleProgressReport);
                FocusConsoleTab(true);
                await ViewModel.ResetEnvironmentAsync(progress);
            }
        }

        private void ExportMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var exportWindow = new ExportWindow.ExportWindow(ViewModel.Environments);
            exportWindow.ShowDialog();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.DetectCurrentEnvironment();
        }
        
        private void EnvironmentsListbox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ApplyEnvironmentButton_OnClick(sender, e);
        }

        private void CurrentEnvironmentVariablesDataGrid_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                foreach (var row in CurrentEnvironmentVariablesDataGrid.SelectedItems)
                {
                    if (row is EnvironmentVariableViewModel environmentVariableViewModel)
                    {

                        ViewModel.SelectedVariable = environmentVariableViewModel;
                        ViewModel.DeleteSelectedVariable();
                        e.Handled = true;
                    }
                }
            }
        }

        private void UndoEditing_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ViewModel.EditingEnabled)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }

            e.Handled = true;
        }

        private void UndoEditing_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.ResetCurrentEnvironmentFromDatasource();
        }

        private void RenameEnvironmentCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!ViewModel.EditingEnabled && ViewModel.SelectedEnvironment != null)
            {
                e.CanExecute = true;
                return;
            }

            e.CanExecute = false;
        }

        private async void OnEnvironmentRenamed(object sender, EnvironmentRenamedEventArgs args)
        {
            var progress = new Progress<CmdExecutorProgress>(HandleProgressReport);
            FocusConsoleTab();
            await ViewModel.RenameEnvironment(args.NewName, progress);
        }

        private void RenameEnvironmentCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var currentName = ViewModel.SelectedEnvironment.Name;
            var childWindow = new RenameEnvironmentWindow.RenameEnvironmentWindow(currentName);
            childWindow.EnvironmentRenamed += OnEnvironmentRenamed;
            childWindow.ShowDialog();
        }
    }
}