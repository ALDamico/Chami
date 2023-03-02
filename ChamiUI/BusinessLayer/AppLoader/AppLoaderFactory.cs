using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Threading.Tasks;
using Chami.CmdExecutor;
using ChamiDbMigrations.Migrations;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.BusinessLayer.Converters;
using ChamiUI.BusinessLayer.EnvironmentHealth;
using ChamiUI.BusinessLayer.EnvironmentHealth.Strategies;
using ChamiUI.BusinessLayer.Exceptions;
using ChamiUI.BusinessLayer.Factories;
using ChamiUI.BusinessLayer.Logger;
using ChamiUI.BusinessLayer.Services;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;
using ChamiUI.Utils;
using ChamiUI.Windows.AboutBox;
using ChamiUI.Windows.DetectedApplicationsWindow;
using ChamiUI.Windows.EnvironmentHealth;
using ChamiUI.Windows.ExportWindow;
using ChamiUI.Windows.ImportEnvironmentWindow;
using ChamiUI.Windows.MainWindow;
using ChamiUI.Windows.MassUpdateWindow;
using ChamiUI.Windows.NewEnvironmentWindow;
using ChamiUI.Windows.NewTemplateWindow;
using ChamiUI.Windows.RenameEnvironmentWindow;
using ChamiUI.Windows.SettingsWindow;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Providers;

namespace ChamiUI.BusinessLayer.AppLoader;

public static class AppLoaderFactory
{
    private static Task ConfigureDatabase(IServiceCollection serviceCollection)
    {
        serviceCollection.AddFluentMigratorCore()
            .ConfigureRunner(r =>
                r.AddSQLite().WithGlobalConnectionString(AppUtils.GetConnectionString()).ScanIn(typeof(Initial).Assembly).For
                    .Migrations());
        return Task.CompletedTask;
    }

    private static Task RegisterServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<MassUpdateService>();
        serviceCollection.AddTransient<ExportService>();
        return Task.CompletedTask;
    }

    private static Task RegisterViewModels(IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<MainWindowViewModel>()
            .AddTransient<SettingsWindowViewModel>()
            .AddTransient<MassUpdateWindowViewModel>()
            .AddTransient<NewEnvironmentViewModel>()
            .AddTransient<DetectedApplicationsViewModel>()
            .AddTransient<ImportEnvironmentWindowViewModel>()
            .AddTransient<ExportWindowViewModel>()
            .AddTransient<NewTemplateWindowViewModel>()
            .AddTransient(sp =>
            {
                var initialName = sp.GetRequiredService<MainWindowViewModel>().SelectedEnvironment.Name;
                return new RenameEnvironmentViewModel(initialName);
            })
            ;
        return Task.CompletedTask;
    }

    public static Task RegisterWindows(IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<MainWindow>()
            .AddTransient<MassUpdateWindow>()
            .AddTransient<SettingsWindow>()
            .AddTransient(sp =>
                new NewEnvironmentWindow(sp.GetRequiredService<MainWindow>(),
                    sp.GetService<NewEnvironmentViewModel>()))
            .AddTransient(sp => new AboutBox(sp.GetRequiredService<MainWindow>()))
            .AddTransient(sp =>
            {
                var window = new DetectedApplicationsWindow(sp.GetRequiredService<DetectedApplicationsViewModel>());
                window.Owner = sp.GetRequiredService<MainWindow>();
                return window;
            })
            .AddTransient<EnvironmentHealthWindow>()
            .AddTransient<ExportWindow>()
            .AddTransient<ImportEnvironmentWindow>()
            .AddTransient<NewTemplateWindow>()
            .AddTransient<RenameEnvironmentWindow>()
            ;
        return Task.CompletedTask;
    }

    private static Task InitLogger(IServiceCollection serviceCollection)
    {
        var chamiLogger = new ChamiLogger();
        chamiLogger.AddFileSink(AppUtils.GetLogFilePath());
        chamiLogger.AddDebugSink();

        Log.Logger = chamiLogger.GetLogger();
        serviceCollection.AddLogging(l => l.AddSerilog());
        return Task.CompletedTask;
    }

    private static Task InitCmdExecutorMessages(IServiceProvider serviceCollection)
    {
        CmdExecutorBase.StartingExecutionMessage = ChamiUIStrings.StartingExecutionMessage;
        CmdExecutorBase.CompletedExecutionMessage = ChamiUIStrings.ExecutionCompleteMessage;
        CmdExecutorBase.UnknownProcessAlreadyExited = ChamiUIStrings.UnknownProcessAlreadyExited;
        CmdExecutorBase.KnownProcessTerminated = ChamiUIStrings.KnownProcessTerminated;
        CmdExecutorBase.KnownProcessAlreadyExited = ChamiUIStrings.KnownProcessAlreadyExited;
        return Task.CompletedTask;
    }

    private static Task RegisterSettingsModule(IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddTransient(_ => new SettingsDataAdapter(AppUtils.GetConnectionString()))
            .AddSingleton(sp =>
            {
                var settingsDataAdapter = sp.GetRequiredService<SettingsDataAdapter>();
                var watchedApplicationDataAdapter = sp.GetRequiredService<WatchedApplicationDataAdapter>();
                var applicationLanguageDataAdapter = sp.GetRequiredService<ApplicationLanguageDataAdapter>();
                return SettingsViewModelFactory.GetSettings(settingsDataAdapter, watchedApplicationDataAdapter, applicationLanguageDataAdapter);
            });
        return Task.CompletedTask;
    }

    private static Task MigrateDatabase(IServiceProvider serviceProvider)
    {
        try
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
        catch (SQLiteException ex)
        {
            Log.Logger.Fatal(ex, "Fatal error while trying to apply database migrations");
        }

        return Task.CompletedTask;
    }

    private static async Task InitLocalization(IServiceProvider serviceProvider)
    {
        var localizationProvider = ResxLocalizationProvider.Instance;
        var dataAdapter = serviceProvider.GetRequiredService<ApplicationLanguageDataAdapter>();
        var languages = await dataAdapter.GetAllAvailableCultureInfosAsync();
        var settingsViewModel = serviceProvider.GetRequiredService<SettingsViewModel>();
        localizationProvider.SearchCultures = new List<CultureInfo>();
        foreach (var cultureInfo in languages)
        {
            localizationProvider.SearchCultures.Add(cultureInfo);
            localizationProvider.AvailableCultures.Add(cultureInfo);
        }

        var currentCulture = dataAdapter.GetCultureInfoByCode(settingsViewModel.LanguageSettings.CurrentLanguage.Code);
        LocalizeDictionary.Instance.Culture = currentCulture;
        ChamiUIStrings.Culture = currentCulture;
    }

    private static Task RegisterDataAdapters(IServiceCollection serviceCollection)
    {
        // The settings data adapter is registered in its own function
        serviceCollection.AddTransient(_ => new EnvironmentDataAdapter(AppUtils.GetConnectionString()))
            .AddTransient(_ => new WatchedApplicationDataAdapter(AppUtils.GetConnectionString()))
            .AddTransient(_ => new ApplicationLanguageDataAdapter(AppUtils.GetConnectionString()))
            .AddTransient(_ => new EnvironmentExportConverter());
        return Task.CompletedTask;
    }

    private static Task RegisterExceptionHandler(IServiceCollection serviceCollection)
    {
        AppUtils.GetChamiApp().DispatcherUnhandledException += ExceptionWindowFactory.ShowExceptionMessageBox;
        return Task.CompletedTask;
    }

    public static void InitAppLoader(AppLoader appLoader)
    {
        appLoader.AddCommand(new DefaultAppLoaderCommand(InitLogger, "Initializing logger"));
        appLoader.AddCommand(new DefaultAppLoaderCommand(ConfigureDatabase, "Configuring database connection"));
        appLoader.AddCommand(new DefaultAppLoaderCommand(RegisterDataAdapters, "Registering data adapters"));
        appLoader.AddCommand(new DefaultAppLoaderCommand(RegisterServices, "Registering services"));
        appLoader.AddCommand(new DefaultAppLoaderCommand(RegisterViewModels, "Registering viewmodels"));
        appLoader.AddCommand(new DefaultAppLoaderCommand(RegisterWindows, "Registering windows"));
        appLoader.AddCommand(new DefaultAppLoaderCommand(RegisterSettingsModule, "Registering settings module"));
        appLoader.AddCommand(new DefaultAppLoaderCommand(InitHealthChecker, "Initializing health checker module"));
#if !DEBUG
        appLoader.AddCommand(
            new DefaultAppLoaderCommand(RegisterExceptionHandler, "Registering exception handler"));
#endif
        
        appLoader.AddPostBuildCommand(new DefaultAppLoaderCommand(MigrateDatabase, "Migrating database"));
        appLoader.AddPostBuildCommand(new DefaultAppLoaderCommand(InitLocalization,
           "Initializing localization support"));
        appLoader.AddPostBuildCommand(new DefaultAppLoaderCommand(InitCmdExecutorMessages,
            "Initializing CMD executor messages"));
    }

    public static Task InitHealthChecker(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton((sp) =>
                new EnvironmentHealthCheckerConfiguration()
                {
                    MaxScore = 1.0,
                    MismatchPenalty = 0.25,
                    CheckInterval = sp.GetRequiredService<SettingsViewModel>().HealthCheckSettings
                        .TimeToCheck.TotalMilliseconds
                })
            .AddSingleton((sp) => new EnvironmentHealthChecker(
                sp.GetService<EnvironmentHealthCheckerConfiguration>(), new DefaultHealthCheckerStrategy())
            );

        return Task.CompletedTask;
    }
}