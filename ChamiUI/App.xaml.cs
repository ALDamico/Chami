using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.BusinessLayer.Logger;
using ChamiUI.PresentationLayer.ViewModels;
using Serilog.Core;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using ChamiDbMigrations;
using ChamiUI.Taskbar;
using ChamiUI.Windows.MainWindow;
using Hardcodet.Wpf.TaskbarNotification;

namespace ChamiUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
#if !DEBUG
            DispatcherUnhandledException += ShowExceptionMessageBox;
#endif
            _taskbarIcon = (TaskbarIcon) FindResource("ChamiTaskbarIcon");
            
            Logger = new ChamiLogger();
            Logger.AddFileSink("chami.log");
            MigrateDatabase();
            try
            {
                Settings = new SettingsDataAdapter(GetConnectionString()).GetSettings();
                var watchedApplications =
                    new WatchedApplicationDataAdapter(GetConnectionString()).GetActiveWatchedApplications();
                Settings.WatchedApplicationSettings.WatchedApplications = new ObservableCollection<WatchedApplicationViewModel>(watchedApplications);
            }
            catch (SQLiteException)
            {
                /*MessageBox.Show("The application database chami.db hasn't been found!\nThe application will now exit.",
                    "Unable to find database!", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(-5);*/
                MigrateDatabase();
            }
            
        }

        private void MigrateDatabase()
        {
            var connection = new SQLiteConnection(GetConnectionString());
            var currentDirectory = Directory.GetCurrentDirectory();
            currentDirectory += "/DataLayer/Db/Migrations";
            var migrationExecutor = new DatabaseMigrationExecutor(connection, currentDirectory);
            migrationExecutor.Migrate();
        }

        public ChamiLogger Logger { get; }

        public SettingsViewModel Settings { get; set; }

        public static string GetConnectionString()
        {
            var chamiDirectory = Environment.CurrentDirectory;
            return String.Format(ConfigurationManager.ConnectionStrings["Chami"].ConnectionString, chamiDirectory);
        }

        public void ShowExceptionMessageBox(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            var exceptionMessage = args.Exception.Message;
            args.Handled = true;
            MessageBox.Show(exceptionMessage, "An exception occurred!", MessageBoxButton.OK, MessageBoxImage.Error);
            if (Settings.LoggingSettings.LoggingEnabled)
            {
                var logger = Logger.GetLogger();
                logger.Error(exceptionMessage);
                logger.Error(args.Exception.StackTrace);
            }
        }

        public Logger GetLogger()
        {
            return Logger.GetLogger();
        }

        private TaskbarIcon _taskbarIcon;

        private async void App_OnStartup(object sender, StartupEventArgs e)
        {
            MainWindow = new MainWindow();
            if (_taskbarIcon != null)
            {
                (MainWindow.DataContext as MainWindowViewModel).EnvironmentChanged +=
                    (_taskbarIcon.DataContext as TaskbarBehaviourViewModel).OnEnvironmentChanged;
            }
            MainWindow.Show();
        }
    }
}