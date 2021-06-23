using ChamiDbMigrations.Entities;

namespace ChamiUI.BusinessLayer.Converters
{
    public interface ISettingConverter<T>
    {
        T Convert(Setting setting);
    }
}