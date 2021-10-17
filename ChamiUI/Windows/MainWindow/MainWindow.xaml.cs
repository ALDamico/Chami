using ChamiUI.BusinessLayer.Factories;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Animation;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.Factories;
using ChamiUI.PresentationLayer.Utils;
using System.Windows.Data;
using Chami.CmdExecutor.Progress;
using ChamiUI.PresentationLayer.Filtering;

namespace ChamiUI.Windows.MainWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Constructs a new <see cref="MainWindow"/> and sets its DataContext, plus registering event handlers.
        /// </summary>
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

        private MainWindowViewModel ViewModel { get; set; }


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
            ConsoleProgressBar.BeginAnimation(RangeBase.ValueProperty, null);
            ConsoleProgressBar.Value = 0.0;

            ConsoleProgressBar.Foreground = ResourceUtils.DefaultProgressBarColor;
        }

        private async void ApplyEnvironmentButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsChangeInProgress)
            {
                return;
            }
            ViewModel.CanUserInterrupt = true;
            if (ViewModel.ExecuteButtonPlayEnabled && !ViewModel.IsChangeInProgress)
            {
                ResetProgressBar();
                FocusConsoleTab();
                var previousEnvironment = ViewModel.ActiveEnvironment;
                Action<CmdExecutorProgress> progress = HandleProgressReport;
                try
                {
                    await ViewModel.ChangeEnvironmentAsync(progress);
                    var watchedApplicationSettings = ViewModel.Settings.WatchedApplicationSettings;
                    if (watchedApplicationSettings.IsDetectionEnabled)
                    {
                        ViewModel.DetectApplicationsAndShowWindow();
                    }
                }
                catch (Exception ex) when (ex is TaskCanceledException or OperationCanceledException)
                {
                    (Application.Current as App)?.Logger.GetLogger().Information("{Message}", ex.Message);
                    (Application.Current as App)?.Logger.GetLogger().Information("{StackTrace}", ex.StackTrace);
                    PrintTaskCancelledMessageToConsole();
                    ViewModel.SelectedEnvironment = previousEnvironment;
                    ViewModel.CanUserInterrupt = false;
                    await ViewModel.ChangeEnvironmentAsync(progress);
                    ViewModel.CanUserInterrupt = true;
                }
            }
            else
            {
                ViewModel.CancelActiveTask();
            }
        }

        private void PrintTaskCancelledMessageToConsole()
        {
            SystemSounds.Exclamation.Play();
            ConsoleTextBox.Text += ChamiUIStrings.OperationCanceledMessage;
            ConsoleTextBox.Text += "Reverting back to previous environment.";
            ConsoleProgressBar.Foreground = System.Windows.Media.Brushes.Red;
        }

        private void FocusConsoleTab(bool clearTextBox = true)
        {
            if (clearTextBox)
            {
                ConsoleTextBox.Text = "";
            }

            TabControls.SelectedIndex = ConsoleTabIndex;
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
            ConsoleProgressBar.BeginAnimation(RangeBase.ValueProperty, doubleAnimation);
            ConsoleProgressBar.Value = o.Percentage;
        }

        private void OnEnvironmentSaved(object sender, EnvironmentSavedEventArgs args)
        {
            if (args != null)
            {
                if (!ViewModel.CheckEnvironmentExists(args.EnvironmentViewModel))
                {
                    ViewModel.Environments.Add(args.EnvironmentViewModel);
                }
            }
        }

        private void EditEnvironmentMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.EnableEditing();
            FocusEnvironmentVariablesTab();
        }

        private const int EnvironmentVariablesTabIndex = 0;
        private const int ConsoleTabIndex = 1;

        private void FocusEnvironmentVariablesTab()
        {
            TabControls.SelectedIndex = EnvironmentVariablesTabIndex;
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
            e.CanExecute = !ViewModel.EditingEnabled;
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

        private void SettingsMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var childWindow = new SettingsWindow.SettingsWindow(this);
            childWindow.SettingsSaved += OnSettingsSaved;
            childWindow.ShowDialog();
        }

        private void OnSettingsSaved(object sender, SettingsSavedEventArgs args)
        {
            ViewModel.Settings = args.Settings;

            ((App) Application.Current).Settings = args.Settings;
            ((App) Application.Current)?.InitLocalization();
        }

        private void BackupEnvironmentMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.BackupEnvironment();
        }

        private void AboutMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            new AboutBox.AboutBox(this).ShowDialog();
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
            var childWindow = new NewEnvironmentWindow.NewEnvironmentWindow(this);
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
                FocusConsoleTab();
                await ViewModel.ResetEnvironmentAsync(HandleProgressReport, CancellationToken.None);
            }
        }

        private void ExportMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var exportWindow = new ExportWindow.ExportWindow(this, ViewModel.Environments);
            exportWindow.ShowDialog();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.DetectCurrentEnvironment();
        }

        public void ResumeState()
        {
            var settings = ViewModel.Settings.MainWindowBehaviourSettings;
            Width = settings.Width;
            Height = settings.Height;
            Top = settings.YPosition;
            Left = settings.XPosition;
            WindowState = settings.WindowState;
            Resources.TryGetCollectionViewSource("EnvironmentsViewSource", out var collectionViewSource);
            var sortDescription = ViewModel.Settings.MainWindowBehaviourSettings.SortDescription;
            collectionViewSource.SortDescriptions.Add(sortDescription);
            switch (sortDescription.PropertyName)
            {
                default:
                    SortByIdRadioButton.IsChecked = true;
                    break;
                case "Name":
                    SortByNameRadioButton.IsChecked = true;
                    break;
                case "AddedOn":
                    SortByDateAddedRadioButton.IsChecked = true;
                    break;
            }

            if (sortDescription.PropertyName == "Id")
            {
                SortByIdRadioButton.IsChecked = true;
            }

            ViewModel.IsDescendingSorting = sortDescription.Direction == ListSortDirection.Descending;
            ViewModel.FilterStrategy = ViewModel.FilterStrategies.FirstOrDefault(fs =>
                fs.GetType().FullName == settings.SearchPath.GetType().FullName);
        }

        private void EnvironmentsListboxItem_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
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
            e.CanExecute = ViewModel.EditingEnabled;

            e.Handled = true;
        }

        private void UndoEditing_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.ResetCurrentEnvironmentFromDatasource();
            ViewModel.DisableEditing();
        }

        private void MainWindow_OnStateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                var sortDescription = GetCurrentSortDescriptionOrDefault();

                ViewModel.MinimizationStrategy.Minimize(this,
                    () => { ViewModel.SaveWindowState(Width, Height, Left, Top, WindowState, sortDescription); });
            }
        }

        private SortDescription GetCurrentSortDescriptionOrDefault()
        {
            Resources.TryGetCollectionViewSource("EnvironmentsViewSource", out var collectionViewSource);
            SortDescription sortDescription;
            if (collectionViewSource != null && collectionViewSource.SortDescriptions.Count > 0)
            {
                sortDescription = collectionViewSource.SortDescriptions[0];
            }
            else
            {
                sortDescription = new SortDescription("Name", ListSortDirection.Ascending);
            }

            return sortDescription;
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
            FocusConsoleTab();
            await ViewModel.RenameEnvironment(args.NewName, HandleProgressReport);
        }

        private void RenameEnvironmentCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var currentName = ViewModel.SelectedEnvironment.Name;
            var childWindow = new RenameEnvironmentWindow.RenameEnvironmentWindow(this, currentName);
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
                collectionViewSource.View.Refresh();
            }
        }

        private void SortByNameMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            SortByNameRadioButton.IsChecked = true;
            if (ViewModel.IsDescendingSorting)
            {
                ChangeSorting(SortDescriptionUtils.SortByNameDescending);
                return;
            }

            ChangeSorting(SortDescriptionUtils.SortByNameAscending);
        }

        private void SortByIdMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            SortByIdRadioButton.IsChecked = true;
            if (ViewModel.IsDescendingSorting)
            {
                ChangeSorting(SortDescriptionUtils.SortByIdDescending);
                return;
            }

            ChangeSorting(SortDescriptionUtils.SortByIdAscending);
        }

        private void SortDescendingMenuItem_OnChecked(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem)
            {
                return;
            }

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
            SortByDateAddedRadioButton.IsChecked = true;
            if (ViewModel.IsDescendingSorting)
            {
                ChangeSorting(SortDescriptionUtils.SortByDateAddedDescending);
                return;
            }

            ChangeSorting(SortDescriptionUtils.SortByDateAddedAscending);
        }

        private void SortDescendingMenuItem_OnUnchecked(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem)
            {
                return;
            }

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
            var data = e.Data.GetData(DataFormats.FileDrop) as string[];
            DoEnvironmentImporting(data);
        }

        private void DoEnvironmentImporting(string[] filenames)
        {
            List<EnvironmentViewModel> viewModels = ViewModel.StartImportFiles(filenames);

            if (viewModels.Count > 0)
            {
                var importWindow = new ImportEnvironmentWindow.ImportEnvironmentWindow(this);
                importWindow.EnvironmentSaved += OnEnvironmentSaved;
                importWindow.SetEnvironments(viewModels);
                importWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show(ChamiUIStrings.AllFilesRejectedText, ChamiUIStrings.AllFilesRejectedCaption,
                    MessageBoxButton.OK, MessageBoxImage.Error);
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
            string allowedExtensions =
                $"{ChamiUIStrings.DotEnvFileDialogDescription}|*.env|{ChamiUIStrings.JsonFileDialogDescription}|*.json|{ChamiUIStrings.AllSupportedFilesFileDialogDescription}|*.env;*.json";
            var dialog = OpenFileDialogFactory.GetOpenFileDialog(allowedExtensions, true);
            var response = dialog.ShowDialog(this);
            if (response == true)
            {
                DoEnvironmentImporting(dialog.FileNames);
            }
        }

        private void CreateTemplateCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !ViewModel.EditingEnabled;
        }

        private void CreateTemplateCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var newTemplateWindow = new NewTemplateWindow.NewTemplateWindow(this);
            newTemplateWindow.ShowDialog();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            SaveState();
            // Required because otherwise the app won't shutdown properly if it's called by taskbar icon.
            Application.Current.Shutdown(0); 
        }

        public void SaveState()
        {
            var sortDescription = GetCurrentSortDescriptionOrDefault();
            ViewModel.SaveWindowState(Width, Height, Left, Top, WindowState, sortDescription);
        }
    }
}