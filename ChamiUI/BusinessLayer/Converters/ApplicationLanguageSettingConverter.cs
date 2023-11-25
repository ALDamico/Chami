using Chami.Db.Entities;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.PresentationLayer.ViewModels;
using ChamiUI.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace ChamiUI.BusinessLayer.Converters
{
    public class ApplicationLanguageSettingConverter
    {
        public ApplicationLanguageViewModel Convert(Setting value)
        {
            var code = value.Value;
            var dataAdapter = AppUtils.GetAppServiceProvider().GetService<ApplicationLanguageDataAdapter>();
            var res = dataAdapter.GetApplicationLanguageByCode(code);
            return res;
        }
    }
}