using Chami.Db.Entities;

namespace ChamiUI.BusinessLayer.Converters
{
    public interface ISettingConverter<out T>
    {
        T Convert(Setting setting);
    }
}