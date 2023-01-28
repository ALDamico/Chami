using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.BusinessLayer.Logger;
using ChamiUI.PresentationLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Chami.CmdExecutor;
using Chami.CmdExecutor.Commands.Common;
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
using ChamiUI.Interop;
using Serilog.Events;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.PresentationLayer.Progress;
using ChamiUI.Utils;
using ChamiUI.Windows.Exceptions;
using ChamiUI.Windows.Splash;
using SplashScreen = ChamiUI.Windows.Splash;

namespace ChamiUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            _splashScreen = new SplashScreen.SplashScreen();
            _splashScreen.Show();
            _progress = new Progress<AppLoadProgress>(_splashScreen.OnMessageReceived);
            
            _serviceProvider = CreateServices();

            InitializeComponent();
        }

        private SplashScreen.SplashScreen _splashScreen;

        private ChamiLogger InitLogger(bool readSettings = false)
        {
            var chamiLogger = new ChamiLogger();
            chamiLogger.AddFileSink(AppUtils.GetLogFilePath());

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
            CmdExecutorBase.UnknownProcessAlreadyExited = ChamiUIStrings.UnknownProcessAlreadyExited;
            CmdExecutorBase.KnownProcessTerminated = ChamiUIStrings.KnownProcessTerminated;
            CmdExecutorBase.KnownProcessAlreadyExited = ChamiUIStrings.KnownProcessAlreadyExited;
        }

        private readonly IServiceProvider _serviceProvider;
        public IServiceProvider ServiceProvider => _serviceProvider;

        private IServiceProvider CreateServices()
        {
            _progress.Report(new AppLoadProgress(){Message = "Initializing logger", Percentage = 0});
            var chamiLogger = InitLogger();
            Log.Logger = chamiLogger.GetLogger();
            
            _progress.Report(new AppLoadProgress(){Message = "Initializing database migrations", Percentage = 10});
            var serviceCollection = new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(r =>
                    r.AddSQLite().WithGlobalConnectionString(GetConnectionString()).ScanIn(typeof(Initial).Assembly).For
                        .Migrations())
                .AddLogging(l => l.AddSerilog());
                
                _progress.Report(new AppLoadProgress(){Message = "Registering windows", Percentage = 50});
                    
                    serviceCollection
                        .AddSingleton((sp) => new MainWindowViewModel(GetConnectionString()))
                .AddSingleton<MainWindow>();
                
                    _progress.Report(new AppLoadProgress(){Message = "Registering settings module", Percentage = 60});
                serviceCollection
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
            var chamiDirectory = AppUtils.GetApplicationFolder();
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

        private void ShowExceptionMessageBox(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            SystemSounds.Exclamation.Play();
            var exception = args.Exception;
            var exceptionWindow = new ExceptionWindow(exception);
                exceptionWindow.ShowDialog();
            if (Settings.LoggingSettings.LoggingEnabled)
            {
                Log.Logger.Error("{Message}", exception.Message);
                Log.Logger.Error("{Message}", args.Exception.StackTrace);
            }

            if (exceptionWindow.IsApplicationTerminationRequested)
            {
                if (exceptionWindow.IsApplicationRestartRequested)
                {
                    IShellCommand restartCommand = new OpenInExplorerCommand(AppUtils.GetApplicationExecutablePath());
                    restartCommand.Execute();
                }

                Environment.Exit(-1);
            }
#if !DEBUG
            args.Handled = true; // TODO react to user choice
#endif
        }


        private TaskbarIcon _taskbarIcon;

        private void DetectOtherInstance()
        {
            // We don't want this to happen when we're developing (An official release of Chami may be running)
#if !DEBUG
            var processName = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly()?.Location);
            var otherInstances = Process.GetProcessesByName(processName)
                .Where(p => p.Id != Process.GetCurrentProcess().Id).ToArray();
            
            if (otherInstances.Length >= 1)
            {
                var otherInstance = otherInstances[0];
                
                User32Utils.FocusOtherWindowAndExit(otherInstance);
            }
#endif
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var loadTask = Task.Run(() =>
            {
                _progress.Report(new AppLoadProgress() {Message = "Registering exception handler", Percentage = 60});
                DispatcherUnhandledException += ShowExceptionMessageBox;
                try
                {
                    _progress.Report(new AppLoadProgress() {Message = "Migrating database", Percentage = 80});
                    MigrateDatabase();
                }
                catch (SQLiteException ex)
                {
                    Log.Logger.Fatal(ex, "Fatal error while trying to apply database migrations");
                }

                _progress.Report(new AppLoadProgress() {Message = "Initializing localization", Percentage = 81});
                InitLocalization();
                _progress.Report(new AppLoadProgress()
                    {Message = "Initializing environment health check module", Percentage = 85});
                
                InitCmdExecutorMessages();
                _progress.Report(new AppLoadProgress() {Message = "Detecting other Chami instances", Percentage = 90});
                DetectOtherInstance();
                
                InitHealthChecker();


            }).ContinueWith(async t =>
            {
                await t;
                Dispatcher.Invoke(() => { ShowMainWindow(e); });
            });
            

        }

        private void ShowMainWindow(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.ResumeState();
            MainWindow = mainWindow;
            _taskbarIcon = (TaskbarIcon) FindResource("ChamiTaskbarIcon");
            HandleCommandLineArguments(e);

            if (_taskbarIcon != null)
            {
                var viewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
                if (_taskbarIcon.DataContext is TaskbarBehaviourViewModel behaviourViewModel)
                {
                    viewModel.EnvironmentChanged += behaviourViewModel.OnEnvironmentChanged;
                }

                viewModel.EnvironmentChanged += OnEnvironmentChanged;
            }

            _splashScreen.Close();
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
        private IProgress<AppLoadProgress> _progress;
    }
}