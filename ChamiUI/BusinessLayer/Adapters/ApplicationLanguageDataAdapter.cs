using System.Collections.Generic;
using System.Globalization;
using Chami.Db.Repositories;
using ChamiUI.BusinessLayer.Converters;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Adapters
{
    public class ApplicationLanguageDataAdapter
    {
        public ApplicationLanguageDataAdapter(string connectionString)
        {
            _repository = new UiLanguageRepository(connectionString);
        }
        private UiLanguageRepository _repository;

        public ApplicationLanguageViewModel GetApplicationLanguageByCode(string code)
        {
            var converter = new ApplicationLanguageConverter();
            var entity = _repository.GetUiLanguageByCode(code);
            return converter.To(entity);
        }

        public IEnumerable<ApplicationLanguageViewModel> GetAllApplicationLanguages()
        {
            var converter = new ApplicationLanguageConverter();
            var result = _repository.GetAllUiLanguages();
            var output = new List<ApplicationLanguageViewModel>();
            foreach (var uiLanguage in result)
            {
                output.Add(converter.To(uiLanguage));
            }

            return output;
        }

        public IEnumerable<CultureInfo> GetAllAvailableCultureInfos()
        {
            var languages = GetAllApplicationLanguages();
            var result = new List<CultureInfo>();

            foreach (var language in languages)
            {
                CultureInfo ci = CultureInfo.CreateSpecificCulture(language.Code);
                result.Add(ci);
            }
            return result;
        }

        public CultureInfo GetCultureInfoByCode(string code)
        {
            var applicationLanguage = GetApplicationLanguageByCode(code);
            if (applicationLanguage == null)
            {
                return null;
            }

            return CultureInfo.CreateSpecificCulture(applicationLanguage.Code);
        }
    }
}