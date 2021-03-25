using System.Configuration;
using System.Windows;
using ChamiUI;
using ChamiUI.BusinessLayer;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.DataLayer.Entities;
using Xunit;

namespace ChamiTests
{
    public class BusinessTests
    {
        private static string connectionString = "Data Source=D:/code/Chami/ChamiUI/bin/Debug/net5.0-windows/chami.db;Version=3;";
        [Fact]
        public void TestEnvironmentDataAdapterInstantiation()
        {
            var dataAdapter = new EnvironmentDataAdapter("Data Source=|DataDirectory|/DataLayer/Db/chami.db;Version=3");
            Assert.NotNull(dataAdapter);
        }

        [Fact]
        public void TestRealSettings()
        {
            var dataAdapter = new SettingsDataAdapter(connectionString);
            var viewModel = dataAdapter.GetSettings();
            Assert.NotNull(viewModel);
        }

        [Fact]
        public void TestSettingsViewModelConverter()
        {
            Setting[] settings = new[]
            {
                new Setting()
                {
                    
                    Type = "System.Boolean",
                    SettingName = "LoggingEnabled",
                    ViewModelName = "ChamiUI.PresentationLayer.ViewModels.LoggingSettingsViewModel", 
                    Value = "true",
                    PropertyName = "LoggingSettings"
                }
            };
            var dataAdapter = new SettingsDataAdapter(connectionString);
            var viewModel = dataAdapter.ToViewModel(settings);
            Assert.NotNull(viewModel);
            Assert.True(viewModel.LoggingSettings.LoggingEnabled);
        }
    }
}