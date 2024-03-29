﻿using ChamiUI.BusinessLayer.Factories;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
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
using Chami.Db.Entities;
using ChamiUI.PresentationLayer.Filtering;
using ChamiUI.PresentationLayer.ViewModels.State;
using ChamiUI.Utils;
using ChamiUI.Windows.Abstract;
using Serilog;
using ChamiUI.Windows.EnvironmentHealth;
using Microsoft.Extensions.DependencyInjection;

namespace ChamiUI.Windows.MainWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ChamiWindow
    {
        /// <summary>
        /// Constructs a new <see cref="MainWindow"/> and sets its DataContext, plus registering event handlers.
        /// </summary>
        public MainWindow(MainWindowViewModel viewModel)
        {
            ViewModel = viewModel;
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
            Log.Logger.Debug("Attempt to quit application");
            var result = MessageBox.Show(ChamiUIStrings.QuitMessageBoxText, ChamiUIStrings.QuitMessageBoxCaption,
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question);
            Log.Logger.Debug("User responded with {Result}", result);
            if (result == MessageBoxResult.OK)
            {
                Application.Current.Shutdown(0);
            }
        }

        private async void ApplyEnvironmentButton_OnClick(object sender, RoutedEventArgs e)
        {
            await ViewModel.ApplyEnvironmentButtonClickAction(this);
        }

        internal void FocusConsoleTab(bool clearTextBox = true)
        {
            if (clearTextBox)
            {
                ConsoleTextBox.Text = "";
            }

            TabControls.SelectedIndex = ConsoleTabIndex;
        }

        internal void HandleProgressReport(CmdExecutorProgress o)
        {
            if (o.Message != null)
            {
                var message = o.Message.TrimStart('\n');
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

       

        private const int EnvironmentVariablesTabIndex = 0;
        private const int ConsoleTabIndex = 1;

        private void FocusEnvironmentVariablesTab()
        {
            TabControls.SelectedIndex = EnvironmentVariablesTabIndex;
        }

        private void SaveCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.SaveCurrentEnvironment();
        }

        private void NewEnvironmentCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !ViewModel.StateManager.CurrentState.EditingEnabled;
        }

        private void SaveCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ViewModel.StateManager.CurrentState.CanSave;
        }

        private void SettingsMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var childWindow = AppUtils.GetAppServiceProvider().GetRequiredService<SettingsWindow.SettingsWindow>();
            childWindow.Owner = this;
            childWindow.ShowDialog();
        }


        private void BackupEnvironmentMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.BackupEnvironment();
        }

        private void AboutMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            AppUtils.GetAppServiceProvider().GetService<AboutBox.AboutBox>().ShowDialog();
        }

        private void NewEnvironmentCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CreateNewEnvironmentWindow();
        }

        private void CreateNewEnvironmentWindow(EnvironmentViewModel dataContext = null)
        {
            var childWindow = AppUtils.GetAppServiceProvider()
                .GetRequiredService<NewEnvironmentWindow.NewEnvironmentWindow>();
            if (dataContext != null)
            {
                childWindow.SetEnvironment(dataContext);
            }

            childWindow.ShowDialog();
            ViewModel.RefreshEnvironments();
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
            var exportWindow = AppUtils.GetAppServiceProvider().GetService<ExportWindow.ExportWindow>();
            exportWindow.Owner = this;
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
                if (ViewModel.SelectedVariable.ChangeInProgress)
                {
                    return;
                }

                if (!ViewModel.StateManager.CurrentState.EditingEnabled)
                {
                    return;
                }

                foreach (EnvironmentVariableViewModel row in CurrentEnvironmentVariablesDataGrid.SelectedItems)
                {
                    ToggleDeletionInner(row);

                    e.Handled = true;
                }
            }
        }

        private void ToggleDeletionInner(object row)
        {
            if (row is EnvironmentVariableViewModel environmentVariableViewModel)
            {
                ViewModel.ToggleVariableDeletion(environmentVariableViewModel);
            }
        }

        private void UndoEditing_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ViewModel.StateManager.CurrentState.EditingEnabled;

            e.Handled = true;
        }

        private void UndoEditing_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.ResetCurrentEnvironmentFromDatasource();
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
            e.CanExecute = !ViewModel.StateManager.CurrentState.EditingEnabled && ViewModel.SelectedEnvironment != null;
        }

        private void RenameEnvironmentCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var childWindow = AppUtils.GetAppServiceProvider()
                .GetService<RenameEnvironmentWindow.RenameEnvironmentWindow>();
            childWindow.Owner = this;
            childWindow.ShowDialog();
        }

        private void FocusFilterTextboxCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ViewModel.StateManager.CurrentState.EditingEnabled;
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
            SubscribeToFilterEvent("EnvironmentsViewSource");
            SubscribeToFilterEvent("BackupEnvironmentsViewSource");
        }

        private void SubscribeToFilterEvent(string viewSourceName)
        {
            Resources.TryGetCollectionViewSource(viewSourceName, out var collectionViewSource);
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
                var importWindow = AppUtils.GetAppServiceProvider()
                    .GetService<ImportEnvironmentWindow.ImportEnvironmentWindow>();
                importWindow.SetEnvironments(viewModels);
                importWindow.ShowDialog();
                ViewModel.RefreshEnvironments();
            }
            else
            {
                MessageBox.Show(ChamiUIStrings.AllFilesRejectedText, ChamiUIStrings.AllFilesRejectedCaption,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ImportEnvironmentsCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ViewModel.StateManager.CurrentState.CanImportData;
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
            e.CanExecute = !ViewModel.StateManager.CurrentState.EditingEnabled;
        }

        private void CreateTemplateCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var newTemplateWindow = AppUtils.GetAppServiceProvider().GetService<NewTemplateWindow.NewTemplateWindow>();
            newTemplateWindow.Owner = this;
            //newTemplateWindow.EnvironmentSaved += OnEnvironmentSaved;
            newTemplateWindow.ShowDialog();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            ViewModel.StateManager.CurrentState.CloseMainWindow(ViewModel, this, e);
        }

        internal void SaveState()
        {
            var sortDescription = GetCurrentSortDescriptionOrDefault();
            ViewModel.SaveWindowState(Width, Height, Left, Top, WindowState, sortDescription);
            ViewModel.SaveFontSize();
        }

        private void ConsoleClearMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            ConsoleTextBox.Clear();
        }

        private void ConsoleCopyMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ConsoleTextBox.Text))
            {
                return;
            }

            var selectedText = ConsoleTextBox.SelectedText;
            if (string.IsNullOrWhiteSpace(selectedText))
            {
                selectedText = ConsoleTextBox.Text;
            }

            Clipboard.SetText(selectedText);
        }


        private void EnvironmentTypeTabItem_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!e.Source.Equals(EnvironmentTypeTabItem))
            {
                return;
            }

            switch (EnvironmentTypeTabItem.SelectedIndex)
            {
                //case 0:
                default:
                    ViewModel.ChangeTab(EnvironmentType.NormalEnvironment);
                    break;
                case 1:
                    ViewModel.ChangeTab(EnvironmentType.TemplateEnvironment);
                    break;
                case 2:
                    ViewModel.ChangeTab(EnvironmentType.BackupEnvironment);
                    break;
            }
        }

        private void BackupEnvironmentsViewSource_OnFilter(object sender, FilterEventArgs e)
        {
            ViewModel.FilterStrategy.OnFilter(sender, e);
        }

        private void TemplateEnvironmentsViewSource_OnFilter(object sender, FilterEventArgs e)
        {
            ViewModel.FilterStrategy.OnFilter(sender, e);
        }

        private void DeleteEnvironmentVariableCommand_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ViewModel.IsSelectedVariableDeletable();
            if (!ViewModel.StateManager.CurrentState.EditingEnabled)
            {
                e.CanExecute = false;
            }
        }

        private void DeleteEnvironmentVariableCommand_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (var row in CurrentEnvironmentVariablesDataGrid.SelectedItems)
            {
                ToggleDeletionInner(row);

                e.Handled = true;
            }
        }

        private void EditEnvironmentCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ViewModel.StateManager.CurrentState.IsEditable;
        }

        private void EditEnvironmentCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.StateManager.ChangeState(new MainWindowEditingState());
            FocusEnvironmentVariablesTab();
        }

        private void DeleteEnvironmentCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ViewModel.StateManager.CurrentState.CanDeleteEnvironment)
            {
                e.CanExecute = true;
            }
        }

        private void DeleteEnvironmentCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
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

        private void DuplicateEnvironmentCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ViewModel.StateManager.CurrentState.CanDuplicateEnvironment;
        }

        private void DuplicateEnvironmentCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CreateNewEnvironmentWindow(ViewModel.SelectedEnvironment.Clone());
        }

        private void MassUpdateCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ViewModel.StateManager.CurrentState.CanExecuteMassUpdate;
        }

        private void MassUpdateCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var childWindow = AppUtils.GetChamiApp().ServiceProvider.GetRequiredService<MassUpdateWindow.MassUpdateWindow>();
            
            childWindow.ShowDialog();
            ViewModel.RefreshEnvironments();
        }

        private void EnvironmentHealthStatusBarItem_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShowEnvironmentHealthWindow();
        }

        private void ShowEnvironmentHealthWindow()
        {
            var healthWindow = AppUtils.GetAppServiceProvider().GetService<EnvironmentHealthWindow>();

            healthWindow.DataContext = ViewModel.EnvironmentHealth;
            healthWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            healthWindow.Closing += EnvironmentHealthWindowOnClosing;
            healthWindow.Show();
            healthWindow.Focus();
        }

        private async void EnvironmentHealthWindowOnClosing(object sender, CancelEventArgs e)
        {
            var healthWindow = sender as EnvironmentHealthWindow;
            if (healthWindow?.DataContext is EnvironmentHealthViewModel closedWindowViewModel)
            {
                await ViewModel.SaveEnvironmentHealthColumns(closedWindowViewModel);
            }
        }

        private void EnvironmentHealthMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            ShowEnvironmentHealthWindow();
        }

        private void IncreaseFontSizeCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ViewModel.CanIncreaseFontSize();
        }

        private void IncreaseFontSizeCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.IncreaseFontSize();
        }

        private void DecreaseFontSizeCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ViewModel.CanDecreaseFontSize();
        }

        private void DecreaseFontSizeCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.DecreaseFontSize();
        }
    }
}