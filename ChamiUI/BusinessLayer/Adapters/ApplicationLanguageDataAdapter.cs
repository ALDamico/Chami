using System.Collections.Generic;
using ChamiUI.BusinessLayer.Converters;
using ChamiUI.DataLayer.Repositories;
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
    }
}