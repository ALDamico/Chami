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
using Serilog.Events;
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
            _serviceProvider = CreateServices();

            InitializeComponent();
#if !DEBUG
            DispatcherUnhandledException += ShowExceptionMessageBox;
#endif
            InitCmdExecutorMessages();

            try
            {
                MigrateDatabase();
            }
            catch (SQLiteException ex)
            {
                Log.Logger.Fatal(ex, "Fatal error while trying to apply database migrations");
            }
            
            InitHealthChecker();
        }

        private ChamiLogger InitLogger(bool readSettings = false)
        {
            var chamiLogger = new ChamiLogger();
            chamiLogger.AddFileSink("chami.log");

            if (readSettings)
            {
                var settings = _serviceProvider.GetRequiredService<SettingsViewModel>();
                var loggingSettings = settings.LoggingSettings;
                
                var minimumLogLevel = loggingSettings.SelectedMinimumLogLevel?.BackingValue ?? LogEventLevel.Fatal;
                if (loggingSettings.LoggingEnabled)
                {
                    minimumLogLevel = LogEventLevel.Fatal;
                }

                chamiLogger.SetMinumumLevel(minimumLogLevel);
            }

            return chamiLogger;

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
        public IServiceProvider ServiceProvider => _serviceProvider;

        private IServiceProvider CreateServices()
        {
            var chamiLogger = InitLogger();
            Log.Logger = chamiLogger.GetLogger();
            var serviceCollection = new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(r =>
                    r.AddSQLite().WithGlobalConnectionString(GetConnectionString()).ScanIn(typeof(Initial).Assembly).For
                        .Migrations())
                .AddLogging(l => l.AddSerilog())
                .AddSingleton<MainWindow>()
                .AddTransient(serviceProvider => new SettingsDataAdapter(GetConnectionString()))
                .AddSingleton(serviceProvider =>
                {
                    var connectionString = GetConnectionString();
                    return SettingsViewModelFactory.GetSettings(new SettingsDataAdapter(connectionString),
                        new WatchedApplicationDataAdapter(connectionString),
                        new ApplicationLanguageDataAdapter(connectionString));
                });

            return serviceCollection.BuildServiceProvider();
        }

        private void MigrateDatabase()
        {
            var runner = _serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }

        public SettingsViewModel Settings => _serviceProvider.GetRequiredService<SettingsViewModel>();

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
                Log.Logger.Error("{Message}", exceptionMessage);
                Log.Logger.Error("{Message}", args.Exception.StackTrace);
            }
        }


        private TaskbarIcon _taskbarIcon;

        private void DetectOtherInstance()
        {
            // We don't want this to happen when we're developing (An official release of Chami may be running)
#if !DEBUG
            var processName = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly()?.Location);
            var otherInstances = Process.GetProcessesByName(processName)
                .Where(p => p.Id != Process.GetCurrentProcess().Id).ToArray();
            
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
            var mainWindow = _serviceProvider.GetService<MainWindow>();
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

                    viewModel.EnvironmentChanged += OnEnvironmentChanged;
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
            Log.Logger.Information("Chami is exiting");
            if (!_taskbarIcon.IsDisposed)
            {
                _taskbarIcon.Dispose();
            }

            Log.CloseAndFlush();
        }

        public EnvironmentHealthCheckerConfiguration HealthCheckerConfiguration { get; set; }
        private DispatcherTimer _healthCheckerTimer;
        private EnvironmentViewModel _activeEnvironment;
    }
}