using System.Collections.Generic;
using System.Globalization;
using Chami.Db.Entities;
using Chami.Db.Repositories;
using ChamiUI.BusinessLayer.Converters;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Adapters
{
    /// <summary>
    /// Data adapter for <see cref="UiLanguage"/> entities.
    /// </summary>
    public class ApplicationLanguageDataAdapter
    {
        /// <summary>
        /// Constructs a new <see cref="ApplicationLanguageDataAdapter"/> object.
        /// </summary>
        /// <param name="connectionString">The connection string for the internal <see cref="UiLanguageRepository"/>.</param>
        public ApplicationLanguageDataAdapter(string connectionString)
        {
            _repository = new UiLanguageRepository(connectionString);
        }

        private UiLanguageRepository _repository;

        /// <summary>
        /// Gets an <see cref="ApplicationLanguageViewModel"/> by its ISO-639 code.
        /// </summary>
        /// <param name="code">The code of the language to retrieve from the database.</param>
        /// <returns>If a corresponding entity is found in the database, converts and returns it. Otherwise, returns null.</returns>
        public ApplicationLanguageViewModel GetApplicationLanguageByCode(string code)
        {
            var converter = new ApplicationLanguageConverter();
            var entity = _repository.GetUiLanguageByCode(code);
            return converter.To(entity);
        }

        /// <summary>
        /// Gets all the available <see cref="UiLanguage"/> entities from the database, converts them to <see cref="ApplicationLanguageViewModel"/>s, and returns them.
        /// </summary>
        /// <returns>A (possibly empty) list of <see cref="ApplicationLanguageViewModel"/> objects.</returns>
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

        /// <summary>
        /// Gets all the <see cref="ApplicationLanguageViewModel"/>s, converts them to <see cref="CultureInfo"/> objects, and returns them as an <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <returns>A list of all available <see cref="CultureInfo"/> objects.</returns>
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

        /// <summary>
        /// Gets an <see cref="ApplicationLanguageViewModel"/>, converts them to a <see cref="CultureInfo"/>, and returns it.
        /// </summary>
        /// <param name="code">The language code to use to retrieve the desired object from the database.</param>
        /// <returns>If no suitable <see cref="ApplicationLanguageViewModel"/> is found, returns null. Otherwise, returns the object converted to <see cref="CultureInfo"/></returns>
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