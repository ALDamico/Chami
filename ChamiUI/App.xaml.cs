using ChamiUI.PresentationLayer.ViewModels;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ChamiUI.BusinessLayer.AppLoader;
using ChamiUI.BusinessLayer.EnvironmentHealth;
using ChamiUI.BusinessLayer.EnvironmentHealth.Strategies;
using ChamiUI.Taskbar;
using ChamiUI.Windows.MainWindow;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using ChamiUI.PresentationLayer.Events;
using ChamiUI.Utils;
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

        public IServiceProvider ServiceProvider { get; private set; }

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

        private async void App_OnStartup(object sender, StartupEventArgs e)
        {
            DetectOtherInstance();
            AppLoaderFactory.InitAppLoader(_appLoader);
            
            ServiceProvider = await _appLoader.ExecuteAsync();
            await _appLoader.ExecutePostBuildCommandsAsync();
            Dispatcher.Invoke(() => { ShowMainWindow(e); });
        }

        private void ShowMainWindow(StartupEventArgs e)
        {
            var mainWindow = ServiceProvider.GetService<MainWindow>();
            mainWindow.ResumeState();
            MainWindow = mainWindow;
            _taskbarIcon = (TaskbarIcon) FindResource("ChamiTaskbarIcon");
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
        }

        private void OnEnvironmentChanged(object sender, EnvironmentChangedEventArgs e)
        {
            _activeEnvironment = e.NewActiveEnvironment;
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