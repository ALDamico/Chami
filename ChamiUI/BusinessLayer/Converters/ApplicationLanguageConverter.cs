using Chami.Db.Entities;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Converters
{
    /// <summary>
    /// Converts between <see cref="UiLanguage"/> entities and <see cref="ApplicationLanguageViewModel"/> objects.
    /// </summary>
    public class ApplicationLanguageConverter:IConverter<UiLanguage, ApplicationLanguageViewModel>
    {
        /// <summary>
        /// Converts a <see cref="ApplicationLanguageViewModel"/> to an <see cref="UiLanguage"/> entity.
        /// </summary>
        /// <param name="model">The <see cref="ApplicationLanguageViewModel"/> to convert.</param>
        /// <returns>The corresponding <see cref="UiLanguage"/> entity.</returns>
        public UiLanguage From(ApplicationLanguageViewModel model)
        {
            return new UiLanguage() {Code = model.Code, Name = model.Name, FlagPath = model.IconPath};
        }

        /// <summary>
        /// Converts an <see cref="UiLanguage"/> to a <see cref="ApplicationLanguageViewModel"/> entity.
        /// </summary>
        /// <param name="entity">The <see cref="UiLanguage"/> to convert.</param>
        /// <returns>The corresponding <see cref="ApplicationLanguageViewModel"/>.</returns>
        public ApplicationLanguageViewModel To(UiLanguage entity)
        {
            return new ApplicationLanguageViewModel()
                {Code = entity.Code, Name = entity.Name, IconPath = entity.FlagPath};
        }
    }
}