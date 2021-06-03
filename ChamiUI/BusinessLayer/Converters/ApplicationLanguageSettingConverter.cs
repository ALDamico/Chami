using System;
using System.Globalization;
using System.Windows.Data;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.DataLayer.Entities;
using ChamiUI.DataLayer.Repositories;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Converters
{
    public class ApplicationLanguageSettingConverter
    {
        public ApplicationLanguageViewModel Convert(Setting value)
        {
            var code = value.Value;
            var connectionString = App.GetConnectionString();
            var dataAdapter = new ApplicationLanguageDataAdapter(connectionString);
            var res = dataAdapter.GetApplicationLanguageByCode(code);
            return res;
        }
    }
}