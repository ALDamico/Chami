namespace ChamiUI.BusinessLayer.Converters
{
    public interface IConverter<TFrom, TTo>
    {
        TFrom From(TTo model);
        TTo To(TFrom entity);
    }
}