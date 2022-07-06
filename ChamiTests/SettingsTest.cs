using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using Chami.Db.Entities;
using Chami.Db.Utils;
using ChamiUI.PresentationLayer.ViewModels;
using Xunit;

namespace ChamiTests
{
    
    public class SettingsTest
    {
        [Fact]
        public void TestSettingsGeneration()
        {
            var gridViewColumn = new GridViewColumn();
            gridViewColumn.Width = 250;

            var setting = SettingsUtils.MakeSettings(gridViewColumn, new[] {"Width"});

            var settingCollection = setting as Setting[] ?? setting.ToArray();
            Assert.NotEmpty(settingCollection);
            Assert.Equal("250", settingCollection.First().Value);
        }

        [Fact]
        public void TestComplexSettingGeneration()
        {
            var consoleAppearanceViewModel = new ConsoleAppearanceViewModel();
            consoleAppearanceViewModel.BackgroundColor = Brushes.Red;
            consoleAppearanceViewModel.ForegroundColor = Brushes.White;
            consoleAppearanceViewModel.FontFamily = new FontFamily("Arial");
            var settings = SettingsUtils.MakeSettings(consoleAppearanceViewModel);

            Assert.NotEmpty(settings);
        }
    }
}