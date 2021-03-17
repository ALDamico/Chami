using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ChamiUI.BusinessLayer;
using ChamiUI.PresentationLayer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChamiUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IConfiguration AppConfiguration { get; set; }
        public IServiceProvider ServiceProvider { get; set; }

        public App()
        {
        }
        
        private void ConfigureServices(IServiceCollection services)
        {
            // ...
 
            services.AddTransient(typeof(MainWindow));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var configBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            AppConfiguration = configBuilder.Build();
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
 
            this.ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}