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
using ChamiUI.BusinessLayer.AppLoader;
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
            _appLoader = new AppLoader(_splashScreen.OnMessageReceived);

            InitializeComponent();
        }

        private SplashScreen.SplashScreen _splashScreen;
        private AppLoader _appLoader;

        private Task InitLogger(IServiceCollection serviceCollection)
        {
            var chamiLogger = new ChamiLogger();
            chamiLogger.AddFileSink(AppUtils.GetLogFilePath());

            /* if (readSettings)
             {
                 var settings = ServiceProvider.GetRequiredService<SettingsViewModel>();
                 var loggingSettings = settings.LoggingSettings;
 
                 var minimumLogLevel = loggingSettings.SelectedMinimumLogLevel?.BackingValue ?? LogEventLevel.Fatal;
                 if (loggingSettings.LoggingEnabled)
                 {
                     minimumLogLevel = LogEventLevel.Fatal;
                 }
 
                 chamiLogger.SetMinumumLevel(minimumLogLevel);
             }*/

            Log.Logger = chamiLogger.GetLogger();
            serviceCollection.AddLogging(l => l.AddSerilog());
            return Task.CompletedTask;
        }

        private Task InitHealthChecker(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton((sp) =>
                    new EnvironmentHealthCheckerConfiguration()
                    {
                        MaxScore = 1.0,
                        MismatchPenalty = 0.25,
                        CheckInterval = ServiceProvider.GetRequiredService<SettingsViewModel>().HealthCheckSettings
                            .TimeToCheck.TotalMilliseconds
                    })
                .AddSingleton((sp) =>
                {
                    var healthCheckerTimer = new DispatcherTimer();
                    healthCheckerTimer.Interval = TimeSpan.FromMilliseconds(ServiceProvider
                        .GetRequiredService<EnvironmentHealthCheckerConfiguration>().CheckInterval);
                    healthCheckerTimer.Tick += HealthCheckerTimerOnElapsed;
                    return healthCheckerTimer;
                });
            
            return Task.CompletedTask;
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

        private Task InitCmdExecutorMessages(IServiceCollection serviceCollection)
        {
            CmdExecutorBase.StartingExecutionMessage = ChamiUIStrings.StartingExecutionMessage;
            CmdExecutorBase.CompletedExecutionMessage = ChamiUIStrings.ExecutionCompleteMessage;
            CmdExecutorBase.UnknownProcessAlreadyExited = ChamiUIStrings.UnknownProcessAlreadyExited;
            CmdExecutorBase.KnownProcessTerminated = ChamiUIStrings.KnownProcessTerminated;
            CmdExecutorBase.KnownProcessAlreadyExited = ChamiUIStrings.KnownProcessAlreadyExited;
            return Task.CompletedTask;
        }
        public IServiceProvider ServiceProvider { get; private set; }

        private Task ConfigureDatabase(IServiceCollection serviceCollection)
        {
            serviceCollection.AddFluentMigratorCore()
                .ConfigureRunner(r =>
                    r.AddSQLite().WithGlobalConnectionString(GetConnectionString()).ScanIn(typeof(Initial).Assembly).For
                        .Migrations());
            return Task.CompletedTask;
        }

        private Task RegisterWindows(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton((sp) => new MainWindowViewModel(GetConnectionString()))
                .AddSingleton<MainWindow>();
            return Task.CompletedTask;
        }

        private Task RegisterSettingsModule(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddTransient(serviceProvider => new SettingsDataAdapter(GetConnectionString()))
                .AddSingleton(serviceProvider =>
                {
                    var connectionString = GetConnectionString();
                    return SettingsViewModelFactory.GetSettings(new SettingsDataAdapter(connectionString),
                        new WatchedApplicationDataAdapter(connectionString),
                        new ApplicationLanguageDataAdapter(connectionString));
                });
            return Task.CompletedTask;
        }

        private Task MigrateDatabase(IServiceCollection serviceCollection)
        {
            try
            {
                var runner = ServiceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateUp();
            }
            catch (SQLiteException ex)
            {
                Log.Logger.Fatal(ex, "Fatal error while trying to apply database migrations");
            }

            return Task.CompletedTask;
        }

        public SettingsViewModel Settings => ServiceProvider.GetRequiredService<SettingsViewModel>();

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
            if (Settings != null && Settings.LoggingSettings.LoggingEnabled)
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

        private Task RegisterExceptionHandler(IServiceCollection serviceCollection)
        {
            DispatcherUnhandledException += ShowExceptionMessageBox;
            return Task.CompletedTask;
        }


        private async void App_OnStartup(object sender, StartupEventArgs e)
        {
            DetectOtherInstance();
            _appLoader.AddCommand(new DefaultAppLoaderCommand(InitLogger, "Initializing logger"));
            _appLoader.AddCommand(new DefaultAppLoaderCommand(ConfigureDatabase, "Configuring database connection"));
            _appLoader.AddCommand(new DefaultAppLoaderCommand(RegisterWindows, "Registering windows"));
            _appLoader.AddCommand(new DefaultAppLoaderCommand(RegisterSettingsModule, "Registering settings module"));
#if !DEBUG
            _appLoader.AddCommand(
                new DefaultAppLoaderCommand(RegisterExceptionHandler, "Registering exception handler"));
#endif
            _appLoader.AddCommand(new DefaultAppLoaderCommand(InitHealthChecker, "Initializing health checker module"));
            //_appLoader.AddCommand(new BuildServiceProviderLoaderCommand());
            _appLoader.AddCommand(new DefaultAppLoaderCommand(MigrateDatabase, "Migrating database"), true);
            _appLoader.AddCommand(new DefaultAppLoaderCommand(InitLocalization, "Initializing localization support"), true);
            _appLoader.AddCommand(new DefaultAppLoaderCommand(InitCmdExecutorMessages,
                "Initializing CMD executor messages"), true);
            
            ServiceProvider = 

            await Task.Run(_appLoader.ExecuteAsync);
            await Task.Run(_appLoader.ExecutePostBuildCommandsAsync);
            Dispatcher.Invoke(() => { ShowMainWindow(e); });
        }

        private void ShowMainWindow(StartupEventArgs e)
        {
            var mainWindow = ServiceProvider.GetService<MainWindow>();
            mainWindow.ResumeState();
            MainWindow = mainWindow;
            _taskbarIcon = (TaskbarIcon)FindResource("ChamiTaskbarIcon");
            HandleCommandLineArguments(e);

            if (_taskbarIcon != null)
            {
                var viewModel = ServiceProvider.GetRequiredService<MainWindowViewModel>();
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

        internal async Task InitLocalization(IServiceCollection serviceCollection)
        {
            var localizationProvider = ResxLocalizationProvider.Instance;
            var dataAdapter = new ApplicationLanguageDataAdapter(GetConnectionString());
            var languages = await dataAdapter.GetAllAvailableCultureInfosAsync();
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

        public EnvironmentHealthCheckerConfiguration HealthCheckerConfiguration =>
            ServiceProvider.GetRequiredService<EnvironmentHealthCheckerConfiguration>();

        private DispatcherTimer _healthCheckerTimer => ServiceProvider.GetRequiredService<DispatcherTimer>();
        private EnvironmentViewModel _activeEnvironment;
    }
}