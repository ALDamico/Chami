using System.Configuration;
using System.Windows;
using ChamiUI;
using ChamiUI.BusinessLayer;
using Xunit;

namespace ChamiTests
{
    
    public class BusinessTests
    {
        [Fact]
        public void TestEnvironmentDataAdapterInstantiation()
        {
            
            var dataAdapter = new EnvironmentDataAdapter("Data Source=|DataDirectory|/DataLayer/Db/chami.db;Version=3");
            Assert.NotNull(dataAdapter);
        }
    }
}