using ChamiDbMigrations.Entities;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Converters
{
    public class ApplicationLanguageConverter:IConverter<UiLanguage, ApplicationLanguageViewModel>
    {
        public UiLanguage From(ApplicationLanguageViewModel model)
        {
            return new UiLanguage() {Code = model.Code, Name = model.Name, FlagPath = model.IconPath};
        }

        public ApplicationLanguageViewModel To(UiLanguage entity)
        {
            return new ApplicationLanguageViewModel()
                {Code = entity.Code, Name = entity.Name, IconPath = entity.FlagPath};
        }
    }
}