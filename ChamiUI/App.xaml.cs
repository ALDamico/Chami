using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.BusinessLayer.Logger;
using ChamiUI.PresentationLayer.ViewModels;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using ChamiDbMigrations;
using ChamiDbMigrations.Migrations;
using ChamiUI.Localization;
using ChamiUI.Taskbar;
using ChamiUI.Windows.MainWindow;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Logging;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Providers;

namespace ChamiUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            _serviceProvider = CreateServices();
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
                Settings.WatchedApplicationSettings.WatchedApplications =
                    new ObservableCollection<WatchedApplicationViewModel>(watchedApplications);
                var availableLanguages =
                    new ApplicationLanguageDataAdapter(GetConnectionString()).GetAllApplicationLanguages();
                Settings.LanguageSettings.AvailableLanguages =
                    new ObservableCollection<ApplicationLanguageViewModel>(availableLanguages);
            }
            catch (SQLiteException)
            {
                /*MessageBox.Show("The application database chami.db hasn't been found!\nThe application will now exit.",
                    "Unable to find database!", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(-5);*/
                MigrateDatabase();
            }
        }

        private IServiceProvider _serviceProvider;

        private IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(r =>
                    r.AddSQLite().WithGlobalConnectionString(GetConnectionString()).ScanIn(typeof(Initial).Assembly).For
                        .Migrations())
                .AddLogging(l => l.AddFluentMigratorConsole()).BuildServiceProvider();
        }

        private void MigrateDatabase()
        {
            var runner = _serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
            /*
            var connection = new SQLiteConnection(GetConnectionString());
            var currentDirectory = Directory.GetCurrentDirectory();
            currentDirectory += "/DataLayer/Db/Migrations";
            var migrationExecutor = new DatabaseMigrationExecutor(connection, currentDirectory);
            migrationExecutor.Migrate();
            */
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
            MessageBox.Show(exceptionMessage, ChamiUIStrings.GenericExceptionMessageBoxCaption, MessageBoxButton.OK,
                MessageBoxImage.Error);
            if (Settings.LoggingSettings.LoggingEnabled)
            {
                var logger = Logger.GetLogger();
                logger.Error("{Message}", exceptionMessage);
                logger.Error("{Message}", args.Exception.StackTrace);
            }
        }

        public Logger GetLogger()
        {
            return Logger.GetLogger();
        }

        private readonly TaskbarIcon _taskbarIcon;

        private void DetectOtherInstance()
        {
            var processName = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly()?.Location);
            var otherInstances = Process.GetProcessesByName(processName)
                .Where(p => p.Id != Process.GetCurrentProcess().Id).ToArray();
            if (otherInstances.Length > 1)
            {
                var otherInstance = otherInstances[0];
                var messageBoxText = string.Format(ChamiUIStrings.ExistingInstanceMessageBoxText, otherInstance.Id);
                var messageBoxCaption = ChamiUIStrings.ExistingEnvironmentMessageBoxCaption;
                MessageBox.Show(messageBoxText, messageBoxCaption, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Environment.Exit(0);
            }
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            InitLocalization();
            DetectOtherInstance();
            MainWindow = new MainWindow();
            if (_taskbarIcon != null)
            {
                if (MainWindow.DataContext is MainWindowViewModel viewModel)
                {
                    if (_taskbarIcon.DataContext is TaskbarBehaviourViewModel behaviourViewModel)
                    {
                        viewModel.EnvironmentChanged += behaviourViewModel.OnEnvironmentChanged;
                    }
                }
            }

            MainWindow.Show();
        }

        internal void InitLocalization()
        {
            var localizationProvider = ResxLocalizationProvider.Instance;
            var dataAdapter = new ApplicationLanguageDataAdapter(GetConnectionString());
            var languages = dataAdapter.GetAllAvailableCultureInfos();
            localizationProvider.SearchCultures = new List<CultureInfo>();
            foreach (var cultureInfo in languages)
            {
                localizationProvider.SearchCultures.Add(cultureInfo);
                localizationProvider.AvailableCultures.Add(cultureInfo);
            }

            var currentCulture = dataAdapter.GetCultureInfoByCode(Settings.LanguageSettings.CurrentLanguage.Code);
            LocalizeDictionary.Instance.Culture = currentCulture;
            ChamiUIStrings.Culture = currentCulture;
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            if (!_taskbarIcon.IsDisposed)
            {
                _taskbarIcon.Dispose();
            }
        }
    }
}