using ChamiUI.BusinessLayer.Factories;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.Progress;
using ChamiUI.PresentationLayer.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.Factories;
using ChamiUI.PresentationLayer.Utils;
using System.Windows.Data;
using ChamiUI.PresentationLayer.Filtering;
using dotenv.net;

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
            Resources.TryGetCollectionViewSource("EnvironmentsViewSource", out var collectionViewSource);
            if (collectionViewSource != null)
            {
                collectionViewSource.SortDescriptions.Add(SortDescriptionUtils.SortByIdAscending);
            }
        }

        private void OnEnvironmentExists(object sender, EnvironmentExistingEventArgs e)
        {
            if (e.Exists)
            {
                MessageBox.Show(ChamiUIStrings.ExistingEnvironmentMessageBoxText,
                    ChamiUIStrings.ExistingEnvironmentMessageBoxCaption, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public MainWindowViewModel ViewModel { get; set; }

        private void QuitApplicationMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(ChamiUIStrings.QuitMessageBoxText, ChamiUIStrings.QuitMessageBoxCaption,
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.OK)
            {
                Environment.Exit(0);
            }
        }

        private void ResetProgressBar()
        {
            //Avoids animating the progressbar when its value is reset to zero.
            ConsoleProgressBar.BeginAnimation(ProgressBar.ValueProperty, null);
            ConsoleProgressBar.Value = 0.0;
        }

        private async void ApplyEnvironmentButton_OnClick(object sender, RoutedEventArgs e)
        {
            ResetProgressBar();
            FocusConsoleTab();
            var progress = new Progress<CmdExecutorProgress>(HandleProgressReport);
            await Task.Run(() => ViewModel.ChangeEnvironmentAsync(progress));
            var watchedApplicationSettings = ViewModel.Settings.WatchedApplicationSettings;
            if (watchedApplicationSettings.IsDetectionEnabled)
            {
                var message = ViewModel.GetDetectedApplicationsMessage();
                if (message != null)
                {
                    MessageBox.Show(message, ChamiUIStrings.DetectorMessageBoxCaption, MessageBoxButton.OK,
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
                var message = o.Message;
                message.TrimStart('\n');
                if (!o.Message.EndsWith("\n"))
                {
                    message += "\n";
                }

                ConsoleTextBox.Text += message;
            }

            if (o.OutputStream != null)
            {
                StreamReader reader = new StreamReader(o.OutputStream);
                ConsoleTextBox.Text += reader.ReadToEnd();
            }

            ConsoleTextBox.ScrollToEnd();
            AnimateProgressBar(o);
        }

        private void AnimateProgressBar(CmdExecutorProgress o)
        {
            var duration = DurationFactory.FromMilliseconds(250);
            DoubleAnimation doubleAnimation = new DoubleAnimation(o.Percentage, duration);
            ConsoleProgressBar.BeginAnimation(ProgressBar.ValueProperty, doubleAnimation);
            ConsoleProgressBar.Value = o.Percentage;
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
                message = string.Format(ChamiUIStrings.DeleteEnvironmentNoVariablesText, selectedEnvironmentName);
            }
            else
            {
                message = string.Format(ChamiUIStrings.DeleteEnvironmentWithVariablesText, selectedEnvironmentName,
                    selectedEnvironmentVariableCount);
            }

            var result = MessageBox.Show(message, ChamiUIStrings.DeleteEnvironmentCaption, MessageBoxButton.OKCancel,
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
            if (ViewModel.SelectedEnvironment != null &&
                ViewModel.EditingEnabled &&
                ViewModel.AreSelectedEnvironmentVariablesValid())
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
                    MessageBox.Show(ChamiUIStrings.JsonDeserializationErrorMessageBoxText,
                        ChamiUIStrings.JsonDeserializationErrorMessageBoxCaption, MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    if (ViewModel.Settings.LoggingSettings.LoggingEnabled)
                    {
                        var logger = ((App.Current) as ChamiUI.App).GetLogger();
                        logger.Error(ex.Message);
                        logger.Error(ex.StackTrace);
                    }
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
            (App.Current as ChamiUI.App).Settings = args.Settings;
            (App.Current as ChamiUI.App).InitLocalization();
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
                var newEnvironmentWindow = new ImportEnvironmentWindow.ImportEnvironmentWindow();
                newEnvironmentWindow.EnvironmentSaved += OnEnvironmentSaved;
                var newEnvironments = new List<EnvironmentViewModel>();
                foreach (var filePath in openFileDialog.FileNames)
                {
                    var newVariables = DotEnv.Fluent().WithEnvFiles(new[] {filePath}).Read();
                    var environmentViewModel = new EnvironmentViewModel();
                    environmentViewModel.Name = filePath;
                    foreach (var variable in newVariables)
                    {
                        var environmentVariable = new EnvironmentVariableViewModel();
                        environmentVariable.Name = variable.Key;
                        environmentVariable.Value = variable.Value;
                        environmentViewModel.EnvironmentVariables.Add(environmentVariable);
                    }
                
                    newEnvironments.Add(environmentViewModel);

                
                    // newEnvironmentWindow.SetEnvironment(environmentViewModel);
                
                }
                newEnvironmentWindow.SetEnvironments(newEnvironments);
                newEnvironmentWindow.ShowDialog();
                //ViewModel.ImportDotEnvMultiple(openFileDialog.FileNames);
            }
        }

        private void AboutMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            new AboutBox.AboutBox().ShowDialog();
        }

        private void WebsiteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ProcessUtils.OpenLinkInBrowser("www.lucianodamico.info");
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
                    ChamiUIStrings.ResetEnvironmentMessageBoxText,
                    ChamiUIStrings.ResetEnvironmentMessageBoxCaption, MessageBoxButton.YesNo, MessageBoxImage.Hand,
                    MessageBoxResult.No);
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

        private void MainWindow_OnStateChanged(object? sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
            }
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
            childWindow.Owner = this;
            childWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            childWindow.EnvironmentRenamed += OnEnvironmentRenamed;
            childWindow.ShowDialog();
        }

        private void GithubLinkMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            ProcessUtils.OpenLinkInBrowser("https://github.com/ALDamico/Chami");
        }

        private void FocusFilterTextboxCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!ViewModel.EditingEnabled)
            {
                e.CanExecute = true;
                return;
            }

            e.CanExecute = false;
        }

        private void FocusFilterTextboxCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            FilterTextbox.Focus();
        }

        private void ChangeSorting(SortDescription sortDescription)
        {
            if (Resources["EnvironmentsViewSource"] is CollectionViewSource collectionViewSource)
            {
                collectionViewSource.SortDescriptions.Clear();
                collectionViewSource.SortDescriptions.Add(sortDescription);
            }
        }

        private void SortByNameMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsDescendingSorting)
            {
                ChangeSorting(SortDescriptionUtils.SortByNameDescending);
                return;
            }

            ChangeSorting(SortDescriptionUtils.SortByNameAscending);
        }

        private void SortByIdMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsDescendingSorting)
            {
                ChangeSorting(SortDescriptionUtils.SortByIdDescending);
                return;
            }

            ChangeSorting(SortDescriptionUtils.SortByIdAscending);
        }

        private void SortDescendingMenuItem_OnChecked(object sender, RoutedEventArgs e)
        {
            ToggleSortDirection();
        }

        private void ToggleSortDirection()
        {
            Resources.TryGetCollectionViewSource("EnvironmentsViewSource", out var collectionViewSource);
            if (collectionViewSource != null)
            {
                var sortDescription = SortDescriptionUtils.GetOppositeSorting(collectionViewSource.SortDescriptions[0]);
                ChangeSorting(sortDescription);
            }
        }

        private void SortByDateMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsDescendingSorting)
            {
                ChangeSorting(SortDescriptionUtils.SortByDateAddedDescending);
                return;
            }

            ChangeSorting(SortDescriptionUtils.SortByDateAddedAscending);
        }

        private void SortDescendingMenuItem_OnUnchecked(object sender, RoutedEventArgs e)
        {
            ToggleSortDirection();
        }

        private void EnvironmentsViewSource_OnFilter(object sender, FilterEventArgs e)
        {
            ViewModel.FilterStrategy.OnFilter(sender, e);
        }

        private void FilterTextbox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Resources.TryGetCollectionViewSource("EnvironmentsViewSource", out var collectionViewSource);
            if (collectionViewSource != null)
            {
                collectionViewSource.Filter += ViewModel.FilterStrategy.OnFilter;
            }
        }

        private void ClearFilterButton_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.FilterText = null;
        }

        private void CaseSensitivityCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            RefreshEnvironmentViewSource();
        }

        private void RefreshEnvironmentViewSource()
        {
            Resources.TryGetCollectionViewSource("EnvironmentsViewSource", out var collectionViewSource);
            if (collectionViewSource != null)
            {
                collectionViewSource.View.Refresh();
            }
        }

        private void FilterStrategySelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var newStrategy = e.AddedItems[0] as IFilterStrategy;
            ViewModel.ChangeFilterStrategy(newStrategy);
            RefreshEnvironmentViewSource();
        }
        
        private void MainWindow_OnDrop(object sender, DragEventArgs e)
        {
            var paths = e.Data.GetData(DataFormats.FileDrop) as string[];
            foreach (var path in paths)
            {
                if (Regex.IsMatch(path, @"\.env$"))
                {
                    Debug.Print("Is env");
                }

                Debug.Print(path);
            }
        }

        private void ImportCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            if (ViewModel.EditingEnabled)
            {
                e.CanExecute = false;
            }
        }

        private void ImportCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var fileFilter = "DotEnv Files|*.env|JSON Files|*.json|All supported files|*.env;*.json";
            var dialog = OpenFileDialogFactory.GetOpenFileDialog(fileFilter, true);
            var result = dialog.ShowDialog();
            if (result.HasValue && result == true)
            {
                foreach (var fileName in dialog.FileNames)
                {
                    FileInfo fInfo = new FileInfo(fileName);
                    
                }
            }
        }

        private void ImportEnvironmentsCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            if (ViewModel.EditingEnabled)
            {
                e.CanExecute = false;
            }
        }

        private void ImportEnvironmentsCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            string allowedExtensions = string.Format("{0}|*.env|{1}|*.json|{2}|*.env;*.json",
                ChamiUIStrings.DotEnvFileDialogDescription, ChamiUIStrings.JsonFileDialogDescription,
                ChamiUIStrings.AllSupportedFilesFileDialogDescription);
            var dialog = OpenFileDialogFactory.GetOpenFileDialog(allowedExtensions, true);
            dialog.ShowDialog(this);
            if (dialog.FileNames.Length > 0)
            {
                ViewModel.StartImportFiles(dialog.FileNames);
            }
        }
    }
}
