using ChamiUI.BusinessLayer;
using ChamiUI.BusinessLayer.Adapters;
using System.Linq;
using System.Windows.Media;
using Chami.Db.Entities;
using Chami.Db.Repositories;
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
            Assert.Equal(12.0, viewModel.ConsoleAppearanceSettings.FontSize);
            // Comparison of colors fails if we don't cast to string
            Assert.Equal(Brushes.Black.ToString(), viewModel.ConsoleAppearanceSettings.BackgroundColor.ToString());
            Assert.Equal(Brushes.White.ToString(), viewModel.ConsoleAppearanceSettings.ForegroundColor.ToString());
            Assert.Equal(new FontFamily("Courier New"), viewModel.ConsoleAppearanceSettings.FontFamily);
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

        [Fact]
        public void TestBackup()
        {
            var environmentRepository = new EnvironmentRepository(connectionString);
            EnvironmentBackupper.Backup(environmentRepository);

            var backedUpEnvironment = environmentRepository.GetBackupEnvironments().FirstOrDefault();


            Assert.NotNull(backedUpEnvironment);
            Assert.Null(backedUpEnvironment.EnvironmentVariables.FirstOrDefault(v => v.Name == "_CHAMI_ENV"));

            // We remove the environment we just added to make sure we can execute this test again.
            environmentRepository.DeleteEnvironmentById(backedUpEnvironment.EnvironmentId);
        }
    }
}