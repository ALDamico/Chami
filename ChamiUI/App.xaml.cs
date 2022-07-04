using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.BusinessLayer.Logger;
using ChamiUI.PresentationLayer.ViewModels;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Threading;
using Chami.CmdExecutor;
using ChamiDbMigrations.Migrations;
using ChamiUI.BusinessLayer.EnvironmentHealth;
using ChamiUI.BusinessLayer.EnvironmentHealth.Strategies;
using ChamiUI.Localization;
using ChamiUI.Taskbar;
using ChamiUI.Windows.MainWindow;
using FluentMigrator.Runner;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Providers;
using ChamiUI.BusinessLayer.Factories;
using ChamiUI.PresentationLayer.Events;

namespace ChamiUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            Logger = new ChamiLogger();
            Logger.AddFileSink("chami.log");
            _serviceProvider = CreateServices();
            InitializeComponent();
#if !DEBUG
            DispatcherUnhandledException += ShowExceptionMessageBox;
#endif
            InitCmdExecutorMessages();
            MigrateDatabase();
            try
            {
                var connectionString = GetConnectionString();
                Settings = SettingsViewModelFactory.GetSettings(new SettingsDataAdapter(connectionString),
                    new WatchedApplicationDataAdapter(connectionString),
                    new ApplicationLanguageDataAdapter(connectionString));
            }
            catch (SQLiteException)
            {
                MigrateDatabase();
            }

            InitHealthChecker();
        }
        
        public void OnSettingsSaved(object sender, SettingsSavedEventArgs args)
        {
            Settings = args.Settings;
            InitLocalization();
            InitHealthChecker();
        }

        private void InitHealthChecker()
        {
            HealthCheckerConfiguration = new EnvironmentHealthCheckerConfiguration()
            {
                MaxScore = 1.0,
                MismatchPenalty = 0.25,
                CheckInterval = Settings.HealthCheckSettings.TimeToCheck.TotalMilliseconds
            };
            _healthCheckerTimer = new DispatcherTimer();
            _healthCheckerTimer.Interval = TimeSpan.FromMilliseconds(HealthCheckerConfiguration.CheckInterval);
            _healthCheckerTimer.Tick += HealthCheckerTimerOnElapsed;
            RestartTimer();
        }

        private void RestartTimer()
        {
            if (_healthCheckerTimer.IsEnabled)
            {
                _healthCheckerTimer.Stop();
            }

            if (Settings.HealthCheckSettings.IsEnabled)
            {
                _healthCheckerTimer.Start();
            }
        }

        private void HealthCheckerTimerOnElapsed(object sender, EventArgs e)
        {
            ExecuteHealthCheck();
            RestartTimer();
        }

        private void ExecuteHealthCheck()
        {
            var healthChecker =
                new EnvironmentHealthChecker(HealthCheckerConfiguration, new DefaultHealthCheckerStrategy());
            healthChecker.HealthChecked += (MainWindow as MainWindow).OnHealthChecked;
            healthChecker.CheckEnvironment(_activeEnvironment);
        }

        private void InitCmdExecutorMessages()
        {
            CmdExecutorBase.StartingExecutionMessage = ChamiUIStrings.StartingExecutionMessage;
            CmdExecutorBase.CompletedExecutionMessage = ChamiUIStrings.ExecutionCompleteMessage;
        }

        private readonly IServiceProvider _serviceProvider;

        private IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(r =>
                    r.AddSQLite().WithGlobalConnectionString(GetConnectionString()).ScanIn(typeof(Initial).Assembly).For
                        .Migrations())
                .AddLogging(l => l.AddSerilog(GetLogger())).BuildServiceProvider();
        }

        private void MigrateDatabase()
        {
            var runner = _serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }

        public ChamiLogger Logger { get; }

        public SettingsViewModel Settings { get; set; }

        public static string GetConnectionString()
        {
            var chamiDirectory = Environment.CurrentDirectory;
            try
            {
                return String.Format(ConfigurationManager.ConnectionStrings["Chami"].ConnectionString, chamiDirectory);
            }
            catch (NullReferenceException)
            {
                // A unit test is running. Use its connection string instead
                return "Data Source=|DataDirectory|InputFiles/chami.db;Version=3;";
            }
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

        private TaskbarIcon _taskbarIcon;

        private void DetectOtherInstance()
        {
            var processName = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly()?.Location);
            var otherInstances = Process.GetProcessesByName(processName)
                .Where(p => p.Id != Process.GetCurrentProcess().Id).ToArray();
            // We don't want this to happen when we're developing (An official release of Chami may be running)
#if !DEBUG
            if (otherInstances.Length >= 1)
            {
                var otherInstance = otherInstances[0];
                var messageBoxText = string.Format(ChamiUIStrings.ExistingInstanceMessageBoxText, otherInstance.Id);
                var messageBoxCaption = ChamiUIStrings.ExistingEnvironmentMessageBoxCaption;
                MessageBox.Show(messageBoxText, messageBoxCaption, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Environment.Exit(0);
            }
#endif
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            InitLocalization();
            DetectOtherInstance();
            var mainWindow = new MainWindow();
            ((MainWindowViewModel) (mainWindow.DataContext)).EnvironmentChanged += OnEnvironmentChanged;
            mainWindow.ResumeState();
            MainWindow = mainWindow;
            _taskbarIcon = (TaskbarIcon) FindResource("ChamiTaskbarIcon");
            HandleCommandLineArguments(e);

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
            if (Settings.HealthCheckSettings.IsEnabled)
            {
                ExecuteHealthCheck();
            }
            
        }

        private void OnEnvironmentChanged(object sender, EnvironmentChangedEventArgs e)
        {
            _activeEnvironment = e.NewActiveEnvironment;
            if (Settings.HealthCheckSettings.IsEnabled)
            {
                ExecuteHealthCheck();
            }
        }

        private void HandleCommandLineArguments(StartupEventArgs e)
        {
            if (e.Args.Length > 0)
            {
                if (e.Args.Any(arg => arg == "/ResetWindowLocation"))
                {
                    MainWindow.Top = 0;
                    MainWindow.Left = 0;
                    MainWindow.Width = 600;
                    MainWindow.Height = 400;
                }
            }
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

        public EnvironmentHealthCheckerConfiguration HealthCheckerConfiguration { get; set; }
        private DispatcherTimer _healthCheckerTimer;
        private EnvironmentViewModel _activeEnvironment;
    }
}