using System;
using System.Globalization;
using System.Windows.Data;
using Chami.Db.Entities;
using ChamiUI.BusinessLayer.Adapters;
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